using System;
using System.Windows.Controls;

namespace Regions.Test.Views;

public partial class PageA : UserControl, INavigationAware
{
    public string Datetime { get; set; }

    public PageA()
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
