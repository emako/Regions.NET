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

        // Test with Singleton lifetime
        //services.AddSingleton<PageA>();
        //services.AddSingleton<PageB>();

        // Test with Region Scoped lifetime
        services.AddScoped<PageA>();
        services.AddScoped<PageB>();

        // Test with Transient lifetime
        //services.AddTransient<PageA>();
        //services.AddTransient<PageB>();

        ServiceProvider = services.BuildServiceProvider()
            .UseRegionServiceProvider()
            .RegisterNavigationRegistry(registry =>
            {
                // Test for ContentControl
                registry.Register<PageA>("/PageA");
                registry.Register<PageB>("/PageB");

                // Test for Grid
                registry.Register<PageA>("/GridA");
                registry.Register<PageB>("/GridB");
            });
    }
}
