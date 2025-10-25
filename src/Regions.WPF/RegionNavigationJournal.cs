using Regions.Abstractions;
using System.Collections.Generic;

namespace Regions;

/// <summary>
/// In-memory navigation journal with back/forward stacks.
/// 区域导航日志：使用内存栈维护前进/后退历史。
/// </summary>
public class RegionNavigationJournal : IRegionNavigationJournal
{
    private readonly Stack<RegionNavigationEntry> _backStack = new();
    private readonly Stack<RegionNavigationEntry> _forwardStack = new();

    /// <inheritdoc />
    public bool CanGoBack => _backStack.Count > 0;

    /// <inheritdoc />
    public bool CanGoForward => _forwardStack.Count > 0;

    /// <inheritdoc />
    public RegionNavigationEntry CurrentEntry { get; protected set; }

    /// <inheritdoc />
    public INavigateAsync NavigationTarget { get; set; }

    /// <inheritdoc />
    public void GoBack()
    {
        if (CanGoBack)
        {
            _forwardStack.Push(CurrentEntry);
            CurrentEntry = _backStack.Pop();
            NavigationTarget?.RequestNavigate(CurrentEntry.Item1, CurrentEntry.Item2);
        }
    }

    /// <inheritdoc />
    public void GoForward()
    {
        if (CanGoForward)
        {
            _backStack.Push(CurrentEntry);
            CurrentEntry = _forwardStack.Pop();
            NavigationTarget?.RequestNavigate(CurrentEntry.Item1, CurrentEntry.Item2);
        }
    }

    /// <inheritdoc />
    public void RecordNavigation(RegionNavigationEntry entry, bool persistInHistory)
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

    /// <inheritdoc />
    public void Clear()
    {
        _backStack.Clear();
        _forwardStack.Clear();
        CurrentEntry = (default, default);
    }
}
