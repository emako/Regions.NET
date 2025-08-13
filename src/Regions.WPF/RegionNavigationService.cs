using System;

namespace Regions;

public class RegionNavigationService : IRegionNavigationService
{
    public IRegion Region { get; set; } = null!;

    public IRegionNavigationJournal Journal { get; set; } = null!;

    public event EventHandler Navigating;

    public event EventHandler Navigated;

    public event EventHandler NavigationFailed;

    public void RequestNavigate(Uri uri, object navigationParameters = null)
    {
        Navigating?.Invoke(this, EventArgs.Empty);

        try
        {
            Journal.RecordNavigation((uri, navigationParameters)!, true);
            Journal.NavigationTarget.RequestNavigate(uri, navigationParameters);
            Navigated?.Invoke(this, EventArgs.Empty);
        }
        catch
        {
            NavigationFailed?.Invoke(this, EventArgs.Empty);
            throw;
        }
    }
}
