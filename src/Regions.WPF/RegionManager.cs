using Microsoft.Extensions.DependencyInjection;
using Regions.Abstractions;
using Regions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Regions;

/// <summary>
/// Default implementation of <see cref="IRegionManager"/>.
/// 区域管理器默认实现：负责注册区域并协调导航与历史。
/// </summary>
public partial class RegionManager : IRegionManager
{
    /// <summary>
    /// Stores regions by their names.
    /// 以名称存储区域的字典。
    /// </summary>
    public Dictionary<string, IRegion> RegionDictionary = [];

    /// <inheritdoc />
    public IEnumerable<object> Regions => RegionDictionary.Values;

    /// <inheritdoc />
    public void ClearRegions()
    {
        RegionDictionary.Clear();
    }

    /// <inheritdoc />
    public IRegionManager RemoveRegion(string regionName)
    {
        RegionDictionary.Remove(regionName);
        return this;
    }

    /// <inheritdoc />
    public IRegionManager AddRegion(string regionName, object view)
    {
        RegionDictionary.Remove(regionName);
        RegionDictionary.Add(regionName, view as IRegion);
        return this;
    }

    /// <inheritdoc />
    public IRegionManager AddToRegion(string regionName, object view)
    {
        // TODO: Consider deleting this method 考虑删除本方法

        if (RegionDictionary.TryGetValue(regionName, out IRegion region))
            region.Add(view, regionName);
        else
            RegionDictionary.Add(regionName, view as IRegion);
        return this;
    }

    /// <inheritdoc />
    public void RequestNavigate(string regionName, Uri target, object navigationParameters = null)
    {
        if (RegionDictionary.TryGetValue(regionName, out IRegion region))
        {
            region.RequestNavigate(target, navigationParameters);
        }
    }

    /// <inheritdoc />
    public void RequestRedirect(string regionName, Uri target, object navigationParameters = null)
    {
        if (RegionDictionary.TryGetValue(regionName, out IRegion region))
        {
            region.Clear();

            if (region.NavigationService is IRegionNavigationService navigationService
                && navigationService.Journal is { } journal)
            {
                journal.Clear();
            }
        }

        RequestNavigate(regionName, target, navigationParameters);
    }

    /// <inheritdoc />
    public void RequestGoBack(string regionName)
    {
        if (RegionDictionary.TryGetValue(regionName, out IRegion region)
            && region.NavigationService is IRegionNavigationService navigationService
            && navigationService.Journal is { } journal
            && journal.CanGoBack)
        {
            if (RegionServiceProvider.ServiceProvider is IServiceProvider serviceProvider)
            {
                if (serviceProvider.GetService(typeof(IServiceCollection)) is IServiceCollection services
                    && serviceProvider.GetService(typeof(INavigationRegistry)) is INavigationRegistry registry)
                {
                    Type type = registry.GetViewType(journal.CurrentEntry.Item1.ToKey());
                    ServiceLifetime? lifeTime = services.GetLifetime(type);

                    if (lifeTime == ServiceLifetime.Transient)
                    {
                        region.Remove(region.GetView(journal.CurrentEntry.Item1.ToKey()));
                    }
                }
            }
            journal.GoBack();
        }
    }

    /// <inheritdoc />
    public void RequestGoForward(string regionName)
    {
        if (RegionDictionary.TryGetValue(regionName, out IRegion region)
            && region.NavigationService is IRegionNavigationService navigationService
            && navigationService.Journal is { } journal
            && journal.CanGoForward)
        {
            journal.GoForward();
        }
    }
}

public partial class RegionManager
{
    /// <summary>
    /// Attached property to declare a region name on a WPF element.
    /// 附加属性：在 WPF 元素上声明区域名称。
    /// </summary>
    public static readonly DependencyProperty RegionNameProperty =
        DependencyProperty.RegisterAttached("RegionName", typeof(string), typeof(RegionManager), new(null, OnRegionNameChanged));

    /// <summary>
    /// Gets the region name from an element.
    /// 从元素获取区域名称。
    /// </summary>
    public static string GetRegionName(DependencyObject obj)
        => (string)obj.GetValue(RegionNameProperty);

    /// <summary>
    /// Sets the region name to an element.
    /// 为元素设置区域名称。
    /// </summary>
    public static void SetRegionName(DependencyObject obj, string value)
        => obj.SetValue(RegionNameProperty, value);

    private static void OnRegionNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is string regionName && !string.IsNullOrEmpty(regionName))
        {
            if (RegionServiceProvider.ServiceProvider is IServiceProvider serviceProvider)
            {
                Region region = new(serviceProvider)
                {
                    Name = regionName,
                    Container = d,
                };
                IRegionManager regionManager = (IRegionManager)serviceProvider.GetService(typeof(IRegionManager));
                regionManager.AddRegion(regionName, region);
            }
        }
    }

    /// <summary>
    /// Attached property to add an element to a region with an initial view.
    /// 附加属性：将元素添加到某个区域，并指定初始视图。
    /// </summary>
    public static readonly DependencyProperty AddToRegionProperty =
        DependencyProperty.RegisterAttached("AddToRegion", typeof(string), typeof(RegionManager), new(null, OnAddToRegionChanged));

    /// <summary>
    /// Gets the AddToRegion value.
    /// 获取 AddToRegion 的值。
    /// </summary>
    public static string GetAddToRegion(DependencyObject obj)
        => (string)obj.GetValue(AddToRegionProperty);

    /// <summary>
    /// Sets the AddToRegion value.
    /// 设置 AddToRegion 的值。
    /// </summary>
    public static void SetAddToRegion(DependencyObject obj, string value)
        => obj.SetValue(AddToRegionProperty, value);

    private static void OnAddToRegionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is string regionNameAndUriOriginalString && !string.IsNullOrEmpty(regionNameAndUriOriginalString))
        {
            string[] parts = regionNameAndUriOriginalString.Split('|', ',');

            if (parts.Length >= 2)
            {
                string regionName = parts[0];
                string uriOriginalString = parts[1];

                if (RegionServiceProvider.ServiceProvider is IServiceProvider serviceProvider)
                {
                    IRegionManager regionManager = (IRegionManager)serviceProvider.GetService(typeof(IRegionManager));
                    IRegion region = regionManager?.Regions
                        .Where(item => item is IRegion region && region.Name == regionName)
                        .FirstOrDefault() as IRegion;

                    region?.Add(d, uriOriginalString);
                }
            }
        }
    }

    /// <summary>
    /// Attached property to store navigation parameters for each region instance.
    /// 附加属性：为每个区域实例存储导航参数。
    /// </summary>
    public static readonly DependencyProperty RegionParameterProperty =
        DependencyProperty.RegisterAttached("RegionParameter", typeof(Dictionary<string, object>), typeof(RegionManager), new PropertyMetadata(null));

    /// <summary>
    /// Gets region parameters from an element.
    /// 从元素获取区域参数。
    /// </summary>
    public static Dictionary<string, object> GetRegionParameter(DependencyObject obj)
        => (Dictionary<string, object>)obj.GetValue(RegionParameterProperty);

    /// <summary>
    /// Sets region parameters to an element.
    /// 为元素设置区域参数。
    /// </summary>
    public static void SetRegionParameter(DependencyObject obj, Dictionary<string, object> value)
        => obj.SetValue(RegionParameterProperty, value);
}
