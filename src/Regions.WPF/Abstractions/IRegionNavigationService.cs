using System;

namespace Regions;

public interface IRegionNavigationService : INavigateAsync
{
    public IRegion Region { get; set; }

    public IRegionNavigationJournal Journal { get; set; }

    public event EventHandler Navigating;

    public event EventHandler Navigated;

    public event EventHandler NavigationFailed;
}
