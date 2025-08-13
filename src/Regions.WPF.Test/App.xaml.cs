using Microsoft.Extensions.DependencyInjection;
using Regions.Test.Views;
using System.Windows;

namespace Regions.Test;

public partial class App : Application, IServiceProvider
{
    public static IServiceProvider? ServiceProvider { get; private set; }

    public object? GetService(Type serviceType)
    {
        return ServiceProvider!.GetService(serviceType);
    }

    public T GetRequiredService<T>() where T : notnull
    {
        return ServiceProvider!.GetRequiredService<T>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        IServiceCollection services = new ServiceCollection();
        services.AddSingleton(services);
        services.AddSingleton<IRegionManager, RegionManager>();
        services.AddSingleton<INavigationRegistry, NavigationRegistry>();
        services.AddScoped<PageA>();
        services.AddScoped<PageB>();

        ServiceProvider = services.BuildServiceProvider()
            .UseRegionServiceProvider()
            .RegisterNavigationRegistry(registry =>
            {
                registry.Register<PageA>("/PageA");
                registry.Register<PageB>("/PageB");
            });
    }
}
