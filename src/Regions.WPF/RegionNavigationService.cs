using System;

namespace Regions;

public class RegionNavigationService : IRegionNavigationService
{
    public IRegion Region { get; set; }

    public IRegionNavigationJournal Journal { get; set; }

    public event EventHandler Navigating;

    public event EventHandler Navigated;

    public event EventHandler NavigationFailed;

    public void RequestNavigate(Uri uri, object navigationParameters = null)
    {
        Navigating?.Invoke(this, EventArgs.Empty);

        try
        {
            Journal.NavigationTarget.RequestNavigate(uri, navigationParameters);
            Journal.RecordNavigation((uri, navigationParameters), Journal.CurrentEntry != (default, default));
            Navigated?.Invoke(this, EventArgs.Empty);
        }
        catch
        {
            NavigationFailed?.Invoke(this, EventArgs.Empty);
            throw;
        }
    }
}
