using System;
using System.Windows;

namespace Regions;

/// <summary>
/// Provides access to the DI service provider for Regions with a fallback to Application.Current.
/// 提供 Regions 的依赖注入服务提供者访问，并在缺省时回退到 Application.Current。
/// </summary>
public static class RegionServiceProvider
{
    /// <summary>
    /// Global service provider for the Regions infrastructure.
    /// Regions 基础设施的全局服务提供者。
    /// </summary>
    public static IServiceProvider ServiceProvider { get; set; }

    /// <summary>
    /// Gets a service by type, trying the configured provider first, then Application.Current.
    /// 通过类型获取服务，优先从已配置的提供者获取，其次尝试 Application.Current。
    /// </summary>
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

    /// <summary>
    /// Gets a required service by generic type parameter.
    /// 获取必需服务的泛型快捷方法。
    /// </summary>
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
