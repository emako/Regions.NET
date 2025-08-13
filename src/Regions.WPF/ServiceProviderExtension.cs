using System;

namespace Regions;

public static class ServiceProviderExtension
{
    public static IServiceProvider UseRegionServiceProvider(this IServiceProvider serviceProvider)
    {
        return RegionServiceProvider.ServiceProvider = serviceProvider;
    }

    public static IServiceProvider RegisterNavigationRegistry<T>(this IServiceProvider serviceProvider, string key)
    {
        INavigationRegistry navigationRegistry = serviceProvider.GetService(typeof(INavigationRegistry)) as INavigationRegistry;
        navigationRegistry.Register<T>(key);
        return serviceProvider;
    }

    public static IServiceProvider RegisterNavigationRegistry(this IServiceProvider serviceProvider, Action<INavigationRegistry> action)
    {
        INavigationRegistry navigationRegistry = serviceProvider.GetService(typeof(INavigationRegistry)) as INavigationRegistry;
        action?.Invoke(navigationRegistry);
        return serviceProvider;
    }
}
