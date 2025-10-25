using System;
using System.Collections.Generic;

namespace Regions;

/// <summary>
/// Default region implementation that hosts views and delegates navigation.
/// 区域默认实现：承载视图并委托导航服务执行导航。
/// </summary>
public class Region(IServiceProvider serviceProvider) : IRegion
{
    private readonly Dictionary<string, object> _views = [];
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    /// <inheritdoc />
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc />
    public object Container { get; set; }

    /// <inheritdoc />
    public IEnumerable<object> Views => _views.Values;

    /// <inheritdoc />
    public IRegionNavigationService NavigationService
    {
        get => field ??= new RegionNavigationService()
        {
            Journal = new RegionNavigationJournal()
            {
                NavigationTarget = new RegionAdapter(this, _serviceProvider),
            },
        };
    }

    /// <inheritdoc />
    public IRegionManager RegionManager => (IRegionManager)_serviceProvider.GetService(typeof(IRegionManager));

    /// <inheritdoc />
    public IRegionManager Add(object view, string viewName)
    {
        _views[viewName] = view;
        return RegionManager;
    }

    /// <inheritdoc />
    public void Remove(object view)
    {
        foreach (var key in new List<string>(_views.Keys))
        {
            if (_views[key] == view)
                _views.Remove(key);
        }
    }

    /// <inheritdoc />
    public void Clear()
    {
        _views.Clear();
    }

    /// <inheritdoc />
    public object GetView(string viewName)
    {
        if (string.IsNullOrEmpty(viewName))
            return null;
        _views.TryGetValue(viewName, out object view);
        return view!;
    }

    /// <inheritdoc />
    public void RequestNavigate(Uri uri, object navigationParameters)
    {
        NavigationService?.RequestNavigate(uri, navigationParameters);
    }
}
