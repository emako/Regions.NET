using System;
using System.Collections.Generic;

namespace Regions;

/// <summary>
/// Default implementation of <see cref="INavigationRegistry"/> backed by a dictionary.
/// 导航注册表默认实现：基于字典存储键与视图类型的映射。
/// </summary>
public class NavigationRegistry : INavigationRegistry
{
    private readonly Dictionary<string, Type> _views = new(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public void Register<TView>(string key)
    {
        _views[key] = typeof(TView);
    }

    /// <inheritdoc />
    public void Register(Type type, string key)
    {
        _views[key] = type;
    }

    /// <inheritdoc />
    public Type GetViewType(string key)
    {
        if (string.IsNullOrEmpty(key))
            return null;
        return _views.TryGetValue(key, out var type) ? type : null;
    }
}
