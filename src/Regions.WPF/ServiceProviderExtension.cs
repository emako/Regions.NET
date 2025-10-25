using Regions.Abstractions;
using System;

namespace Regions;

/// <summary>
/// Extensions for configuring and using the Regions service provider.
/// ServiceProvider 扩展方法：用于配置与使用 Regions 的依赖注入服务提供者。
/// </summary>
public static class ServiceProviderExtension
{
    /// <summary>
    /// Set the global service provider for Regions and return the provider.
    /// 设置 Regions 的全局服务提供者，并返回该提供者。
    /// </summary>
    public static IServiceProvider UseRegionServiceProvider(this IServiceProvider serviceProvider)
    {
        return RegionServiceProvider.ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// Register a view type into the navigation registry by key.
    /// 通过键将视图类型注册到导航注册表。
    /// </summary>
    public static IServiceProvider RegisterNavigationRegistry<T>(this IServiceProvider serviceProvider, string key)
    {
        INavigationRegistry navigationRegistry = serviceProvider.GetService(typeof(INavigationRegistry)) as INavigationRegistry;
        navigationRegistry.Register<T>(key);
        return serviceProvider;
    }

    /// <summary>
    /// Register multiple entries by providing a configuration action.
    /// 通过配置委托批量进行导航注册。
    /// </summary>
    public static IServiceProvider RegisterNavigationRegistry(this IServiceProvider serviceProvider, Action<INavigationRegistry> action)
    {
        INavigationRegistry navigationRegistry = serviceProvider.GetService(typeof(INavigationRegistry)) as INavigationRegistry;
        action?.Invoke(navigationRegistry);
        return serviceProvider;
    }
}
