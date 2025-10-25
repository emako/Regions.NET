using System.Collections.Generic;

namespace Regions.Abstractions;

/// <summary>
/// Represents a logical region that hosts views and supports navigation.
/// 区域接口：表示承载视图并支持导航的逻辑区域。
/// </summary>
public interface IRegion : INavigateAsync
{
    /// <summary>
    /// Gets or sets the region name.
    /// 区域名称。
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the region container (e.g., a WPF control).
    /// 区域容器（例如某个 WPF 控件）。
    /// </summary>
    public object Container { get; set; }

    /// <summary>
    /// Gets all hosted views.
    /// 获取区域内的所有视图。
    /// </summary>
    public IEnumerable<object> Views { get; }

    /// <summary>
    /// Adds a view into the region with a given name.
    /// 将视图以指定名称添加到区域。
    /// </summary>
    /// <param name="view">The view instance. 视图实例。</param>
    /// <param name="viewName">The unique view name. 视图名称。</param>
    /// <returns>The region manager. 区域管理器。</returns>
    public IRegionManager Add(object view, string viewName);

    /// <summary>
    /// Removes a view from the region.
    /// 从区域移除视图。
    /// </summary>
    public void Remove(object view);

    /// <summary>
    /// Clears all views from the region.
    /// 清空区域中的所有视图。
    /// </summary>
    public void Clear();

    /// <summary>
    /// Gets a view by its name.
    /// 根据名称获取视图。
    /// </summary>
    /// <param name="viewName">The view name. 视图名称。</param>
    /// <returns>The view instance or null. 视图实例或 null。</returns>
    public object GetView(string viewName);

    /// <summary>
    /// Gets the region manager associated with this region.
    /// 获取与此区域关联的区域管理器。
    /// </summary>
    public IRegionManager RegionManager { get; }

    /// <summary>
    /// Gets the region navigation service.
    /// 获取区域导航服务。
    /// </summary>
    public IRegionNavigationService NavigationService { get; }
}
