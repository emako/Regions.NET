using System;
using System.Collections.Generic;

namespace Regions;

public class RegionNavigationJournal : IRegionNavigationJournal
{
    private readonly Stack<(Uri, object)> _backStack = new();
    private readonly Stack<(Uri, object)> _forwardStack = new();

    public bool CanGoBack => _backStack.Count > 0;
    public bool CanGoForward => _forwardStack.Count > 0;
    public (Uri, object) CurrentEntry { get; protected set; }
    public INavigateAsync NavigationTarget { get; set; }

    public void GoBack()
    {
        if (CanGoBack)
        {
            _forwardStack.Push(CurrentEntry);
            CurrentEntry = _backStack.Pop();
            NavigationTarget?.RequestNavigate(CurrentEntry.Item1, CurrentEntry.Item2);
        }
    }

    public void GoForward()
    {
        if (CanGoForward)
        {
            _backStack.Push(CurrentEntry);
            CurrentEntry = _forwardStack.Pop();
            NavigationTarget?.RequestNavigate(CurrentEntry.Item1, CurrentEntry.Item2);
        }
    }

    public void RecordNavigation((Uri, object) entry, bool persistInHistory)
    {
        if (entry == (default, default))
            return;
        if (Equals(CurrentEntry, entry))
            return;
        if (persistInHistory)
            _backStack.Push(CurrentEntry);
        CurrentEntry = entry;
        _forwardStack.Clear();
    }

    public void Clear()
    {
        _backStack.Clear();
        _forwardStack.Clear();
        CurrentEntry = (default, default);
    }
}
