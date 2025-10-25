using System;

namespace Regions;

public interface IRegionNavigationJournal
{
    public bool CanGoBack { get; }

    public bool CanGoForward { get; }

    public (Uri, object) CurrentEntry { get; }

    public INavigateAsync NavigationTarget { get; set; }

    public void GoBack();

    public void GoForward();

    public void RecordNavigation((Uri, object) entry, bool persistInHistory);

    public void Clear();
}
