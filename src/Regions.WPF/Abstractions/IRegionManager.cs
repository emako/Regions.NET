using System;
using System.Collections.Generic;

namespace Regions;

/// <summary>
/// Manages regions and coordinates navigation between views.
/// 区域管理器：管理区域并协调视图之间的导航。
/// </summary>
public interface IRegionManager
{
    /// <summary>
    /// Gets all registered regions.
    /// 获取所有已注册的区域。
    /// </summary>
    public IEnumerable<object> Regions { get; }

    /// <summary>
    /// Adds a view to the specified region.
    /// 将视图添加到指定区域。
    /// </summary>
    /// <param name="regionName">Region name. 区域名称。</param>
    /// <param name="view">View instance. 视图实例。</param>
    /// <returns>The region manager. 区域管理器。</returns>
    public IRegionManager AddToRegion(string regionName, object view);

    /// <summary>
    /// Requests navigation within a region.
    /// 请求在区域内进行导航。
    /// </summary>
    /// <param name="regionName">Region name. 区域名称。</param>
    /// <param name="target">Target URI. 目标 URI。</param>
    /// <param name="navigationParameters">Optional parameters. 可选参数。</param>
    public void RequestNavigate(string regionName, Uri target, object navigationParameters = null);

    /// <summary>
    /// Clears region history and navigates to target.
    /// 清空区域历史并导航到目标。
    /// </summary>
    /// <param name="regionName">Region name. 区域名称。</param>
    /// <param name="target">Target URI. 目标 URI。</param>
    /// <param name="navigationParameters">Optional parameters. 可选参数。</param>
    public void RequestRedirect(string regionName, Uri target, object navigationParameters = null);

    /// <summary>
    /// Navigates back if possible in the region's journal.
    /// 如果可能，在区域导航日志中后退。
    /// </summary>
    public void RequestGoBack(string regionName);

    /// <summary>
    /// Navigates forward if possible in the region's journal.
    /// 如果可能，在区域导航日志中前进。
    /// </summary>
    public void RequestGoForward(string regionName);
}
