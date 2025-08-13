using Microsoft.Extensions.DependencyInjection;
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
    public IRegion Region { get; set; }
    public INavigateAsync NavigationTarget { get; set; }

    public void GoBack()
    {
        if (CanGoBack)
        {
            if (RegionServiceProvider.ServiceProvider is not null)
            {
                if (RegionServiceProvider.ServiceProvider.GetService(typeof(IServiceCollection)) is IServiceCollection services
                    && RegionServiceProvider.ServiceProvider.GetService(typeof(INavigationRegistry)) is INavigationRegistry registry)
                {
                    Type type = registry.GetViewType(CurrentEntry.Item1.OriginalString);
                    ServiceLifetime? lifeTime = services.GetLifetime(type);

                    if (lifeTime != ServiceLifetime.Singleton)
                    {
                        Region.Remove(Region.GetView(CurrentEntry.Item1.OriginalString));
                    }
                }
            }

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

        if (entry.Item2 is INavigationParameters parameters)
        {
            if (parameters.Redirect)
            {
                _backStack.Clear();
                _forwardStack.Clear();
                CurrentEntry = entry;
                return;
            }
        }

        if (persistInHistory)
            _backStack.Push(CurrentEntry);
        CurrentEntry = entry;
        _forwardStack.Clear();
    }

    public void Clear()
    {
        _backStack.Clear();
        _forwardStack.Clear();
        CurrentEntry = default;
    }
}
