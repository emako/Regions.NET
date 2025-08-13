using System;
using System.Windows;

namespace Regions;

public static class RegionServiceProvider
{
    public static IServiceProvider ServiceProvider { get; set; }

    public static object GetService(Type serviceType)
    {
        if (ServiceProvider is not null)
        {
            return ServiceProvider.GetService(serviceType);
        }
        else if (Application.Current is IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService(serviceType);
        }
        return default;
    }

    public static T GetRequiredService<T>() where T : notnull
    {
        if (ServiceProvider is not null)
        {
            return (T)ServiceProvider.GetService(typeof(T));
        }
        else if (Application.Current is IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(typeof(T));
        }
        return default;
    }
}
