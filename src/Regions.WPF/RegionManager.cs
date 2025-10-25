using Microsoft.Extensions.DependencyInjection;
using Regions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Regions;

public partial class RegionManager : IRegionManager
{
    public Dictionary<string, IRegion> RegionDictionary = [];
    public IEnumerable<object> Regions => RegionDictionary.Values;

    public IRegionManager AddToRegion(string regionName, object view)
    {
        if (RegionDictionary.TryGetValue(regionName, out IRegion region))
            region.Add(view, regionName);
        else
            RegionDictionary.Add(regionName, view as IRegion);
        return this;
    }

    public void RequestNavigate(string regionName, Uri target, object navigationParameters = null)
    {
        if (RegionDictionary.TryGetValue(regionName, out IRegion region))
        {
            region.RequestNavigate(target, navigationParameters);
        }
    }

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
    public static readonly DependencyProperty RegionNameProperty =
        DependencyProperty.RegisterAttached("RegionName", typeof(string), typeof(RegionManager), new(null, OnRegionNameChanged));

    public static string GetRegionName(DependencyObject obj)
        => (string)obj.GetValue(RegionNameProperty);

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
                regionManager.AddToRegion(regionName, region);
            }
        }
    }

    public static readonly DependencyProperty AddToRegionProperty =
        DependencyProperty.RegisterAttached("AddToRegion", typeof(string), typeof(RegionManager), new(null, OnAddToRegionChanged));

    public static string GetAddToRegion(DependencyObject obj)
        => (string)obj.GetValue(AddToRegionProperty);

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

    // RegionParameter stores navigation parameters for each region instance
    public static readonly DependencyProperty RegionParameterProperty =
        DependencyProperty.RegisterAttached("RegionParameter", typeof(Dictionary<string, object>), typeof(RegionManager), new PropertyMetadata(null));

    public static Dictionary<string, object> GetRegionParameter(DependencyObject obj)
        => (Dictionary<string, object>)obj.GetValue(RegionParameterProperty);

    public static void SetRegionParameter(DependencyObject obj, Dictionary<string, object> value)
        => obj.SetValue(RegionParameterProperty, value);
}
