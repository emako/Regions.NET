using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Regions;

public class Region(IServiceProvider serviceProvider) : IRegion
{
    private readonly Dictionary<string, object> _views = [];
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public string Name { get; set; } = string.Empty;
    public object Container { get; set; }
    public IEnumerable<object> Views => _views.Values;

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

    public IRegionManager RegionManager => (IRegionManager)_serviceProvider.GetService(typeof(IRegionManager));

    public event PropertyChangedEventHandler PropertyChanged;

    public IRegionManager Add(object view, string viewName)
    {
        _views[viewName] = view;
        return RegionManager;
    }

    public void Remove(object view)
    {
        foreach (var key in new List<string>(_views.Keys))
        {
            if (_views[key] == view)
                _views.Remove(key);
        }
    }

    public void RemoveAll()
    {
        _views.Clear();
    }

    public object GetView(string viewName)
    {
        if (string.IsNullOrEmpty(viewName))
            return null;
        _views.TryGetValue(viewName, out object view);
        return view!;
    }

    public void RequestNavigate(Uri uri, object navigationParameters)
    {
        NavigationService?.RequestNavigate(uri, navigationParameters);
    }
}
