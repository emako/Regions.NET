using System;
using System.Collections.Generic;

namespace Regions;

public interface IRegionManager
{
    public IEnumerable<object> Regions { get; }

    public IRegionManager AddToRegion(string regionName, object view);

    public void RequestNavigate(string regionName, Uri target, object navigationParameters = null);

    public void RequestNavigateBack(string regionName);

    public void RequestNavigateForward(string regionName);
}
