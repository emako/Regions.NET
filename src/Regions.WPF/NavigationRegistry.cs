using System;
using System.Collections.Generic;

namespace Regions;

public class NavigationRegistry : INavigationRegistry
{
    private readonly Dictionary<string, Type> _views = new(StringComparer.OrdinalIgnoreCase);

    public void Register<TView>(string key)
    {
        _views[key] = typeof(TView);
    }

    public void Register(Type type, string key)
    {
        _views[key] = type;
    }

    public Type GetViewType(string key)
    {
        if (string.IsNullOrEmpty(key))
            return null;
        return _views.TryGetValue(key, out var type) ? type : null;
    }
}
