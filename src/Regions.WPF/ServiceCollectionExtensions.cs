using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Regions;

public static class ServiceCollectionExtensions
{
    public static ServiceLifetime? GetLifetime(this IServiceCollection services, Type serviceType)
    {
        var descriptor = services.FirstOrDefault(d => d.ServiceType == serviceType);
        return descriptor?.Lifetime;
    }
}
