using System.Windows.Controls;

namespace Regions.Test.Views;

public partial class PageA : UserControl, INavigationAware
{
    public PageA()
    {
        InitializeComponent();
    }

    public void OnNavigatedFrom((Uri, object) entry)
    {
    }

    public void OnNavigatedTo((Uri, object) entry)
    {
    }
}
