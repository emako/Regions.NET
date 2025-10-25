using Regions.Abstractions;
using System;

namespace Regions;

/// <summary>
/// Default navigation service that raises events and records history.
/// 区域导航服务默认实现：触发导航事件并记录历史。
/// </summary>
public class RegionNavigationService : IRegionNavigationService
{
    /// <inheritdoc />
    public IRegion Region { get; set; }

    /// <inheritdoc />
    public IRegionNavigationJournal Journal { get; set; }

    /// <inheritdoc />
    public event EventHandler Navigating;

    /// <inheritdoc />
    public event EventHandler Navigated;

    /// <inheritdoc />
    public event EventHandler NavigationFailed;

    /// <inheritdoc />
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
