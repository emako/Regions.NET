using System.Collections.Generic;

namespace Regions;

public interface IRegion : INavigateAsync
{
    public string Name { get; set; }

    public object Container { get; set; }

    public IEnumerable<object> Views { get; }

    public IRegionManager Add(object view, string viewName);

    public void Remove(object view);

    public void RemoveAll();

    public object GetView(string viewName);

    public IRegionManager RegionManager { get; }

    public IRegionNavigationService NavigationService { get; }
}
