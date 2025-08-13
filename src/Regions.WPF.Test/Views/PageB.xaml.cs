using System.Windows.Controls;

namespace Regions.Test.Views;

public partial class PageB : UserControl, INavigationAware
{
    public string Datetime { get; set; }

    public PageB()
    {
        Datetime = DateTime.Now.Ticks.ToString();
        DataContext = this;
        InitializeComponent();
    }

    public void OnNavigatedFrom((Uri, object) entry)
    {
    }

    public void OnNavigatedTo((Uri, object) entry)
    {
    }
}
