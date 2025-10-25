using System;

namespace Regions.Abstractions;

/// <summary>
/// Records navigation history and supports back/forward operations.
/// 区域导航日志：记录导航历史并支持前进/后退操作。
/// </summary>
public interface IRegionNavigationJournal
{
    /// <summary>
    /// Whether back navigation is available.
    /// 是否可以后退。
    /// </summary>
    public bool CanGoBack { get; }

    /// <summary>
    /// Whether forward navigation is available.
    /// 是否可以前进。
    /// </summary>
    public bool CanGoForward { get; }

    /// <summary>
    /// Current navigation entry.
    /// 当前导航条目。
    /// </summary>
    public RegionNavigationEntry CurrentEntry { get; }

    /// <summary>
    /// The target that executes navigation actions.
    /// 执行导航动作的目标对象。
    /// </summary>
    public INavigateAsync NavigationTarget { get; set; }

    /// <summary>
    /// Navigate back to the previous entry.
    /// 后退到上一个条目。
    /// </summary>
    public void GoBack();

    /// <summary>
    /// Navigate forward to the next entry.
    /// 前进到下一个条目。
    /// </summary>
    public void GoForward();

    /// <summary>
    /// Records the specified navigation entry.
    /// 记录指定的导航条目。
    /// </summary>
    /// <param name="entry">The entry to record. 要记录的条目。</param>
    /// <param name="persistInHistory">Whether to persist current entry in back history. 是否将当前条目写入后退历史。</param>
    public void RecordNavigation(RegionNavigationEntry entry, bool persistInHistory);

    /// <summary>
    /// Clears all history and the current entry.
    /// 清空所有历史和当前条目。
    /// </summary>
    public void Clear();
}
