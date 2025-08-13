using System.Windows;

namespace Regions.Test;

public partial class MainWindow : Window
{
    private readonly IRegionManager _regionManager;

    public MainWindow()
    {
        InitializeComponent();
        _regionManager = RegionServiceProvider.GetRequiredService<IRegionManager>();
    }

    private void BtnA_Click(object sender, RoutedEventArgs e)
    {
        _regionManager?.RequestNavigate("MainRegion", new Uri("/PageA", UriKind.Relative));
    }

    private void BtnB_Click(object sender, RoutedEventArgs e)
    {
        _regionManager?.RequestNavigate("MainRegion", new Uri("/PageB", UriKind.Relative));
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
    {
        _regionManager?.RequestGoBack("MainRegion");
    }

    private void BtnForward_Click(object sender, RoutedEventArgs e)
    {
        _regionManager?.RequestGoForward("MainRegion");
    }

    private void BtnRedirect_Click(object sender, RoutedEventArgs e)
    {
        _regionManager?.RequestRedirect("MainRegion", new Uri("/PageA", UriKind.Relative));
    }
}
