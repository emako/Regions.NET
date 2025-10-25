using System;

namespace Regions;

/// <summary>
/// Provides navigation services for a region.
/// 区域导航服务：为区域提供导航能力。
/// </summary>
public interface IRegionNavigationService : INavigateAsync
{
    /// <summary>
    /// Gets or sets the region associated with this service.
    /// 获取或设置与该服务关联的区域。
    /// </summary>
    public IRegion Region { get; set; }

    /// <summary>
    /// Gets or sets the navigation journal.
    /// 获取或设置导航日志。
    /// </summary>
    public IRegionNavigationJournal Journal { get; set; }

    /// <summary>
    /// Raised before navigation occurs.
    /// 在发生导航前触发。
    /// </summary>
    public event EventHandler Navigating;

    /// <summary>
    /// Raised after navigation succeeds.
    /// 在导航成功后触发。
    /// </summary>
    public event EventHandler Navigated;

    /// <summary>
    /// Raised when navigation fails.
    /// 在导航失败时触发。
    /// </summary>
    public event EventHandler NavigationFailed;
}
