using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Regions;

/// <summary>
/// Extensions for Microsoft.Extensions.DependencyInjection service collection.
/// ServiceCollection 扩展方法：用于查询服务的生命周期等信息。
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Gets the lifetime for a registered service type.
    /// 获取某注册服务类型的生命周期。
    /// </summary>
    /// <param name="services">Service collection. 服务集合。</param>
    /// <param name="serviceType">Registered service type. 已注册的服务类型。</param>
    /// <returns>The lifetime or null if not registered. 生命周期或 null（若未注册）。</returns>
    public static ServiceLifetime? GetLifetime(this IServiceCollection services, Type serviceType)
    {
        var descriptor = services.FirstOrDefault(d => d.ServiceType == serviceType);
        return descriptor?.Lifetime;
    }
}
