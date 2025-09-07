using System.Windows;

namespace Regions.Test;

public partial class MainWindow : Window
{
    private readonly IRegionManager _regionManager;

    public MainWindow()
    {
        InitializeComponent();
        _regionManager = RegionServiceProvider.GetRequiredService<IRegionManager>();
        //_regionManager.RequestNavigate("MainRegion", new Uri("/PageA", UriKind.Relative));
        _regionManager.RequestNavigate("MainGridRegion", new Uri("/GridA", UriKind.Relative));
    }

    private void BtnA_Click(object sender, RoutedEventArgs e)
    {
        //_regionManager?.RequestNavigate("MainRegion", new Uri("/PageA", UriKind.Relative));
        _regionManager?.RequestNavigate("MainGridRegion", new Uri("/GridA?a=1&b=2", UriKind.Relative));
    }

    private void BtnB_Click(object sender, RoutedEventArgs e)
    {
        //_regionManager?.RequestNavigate("MainRegion", new Uri("/PageB", UriKind.Relative));
        _regionManager?.RequestNavigate("MainGridRegion", new Uri("/GridB?a=1&b=2", UriKind.Relative));
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
    {
        //_regionManager?.RequestGoBack("MainRegion");
        _regionManager?.RequestGoBack("MainGridRegion");
    }

    private void BtnForward_Click(object sender, RoutedEventArgs e)
    {
        //_regionManager?.RequestGoForward("MainRegion");
        _regionManager?.RequestGoForward("MainGridRegion");
    }

    private void BtnRedirect_Click(object sender, RoutedEventArgs e)
    {
        //_regionManager?.RequestRedirect("MainRegion", new Uri("/PageA", UriKind.Relative));
        _regionManager?.RequestRedirect("MainGridRegion", new Uri("/GridA", UriKind.Relative));
    }
}
