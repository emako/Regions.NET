using System;

namespace Regions;

/// <summary>
/// Receives notifications about navigation events for views or view models.
/// 导航感知接口：用于接收视图或视图模型的导航事件通知。
/// </summary>
public interface INavigationAware
{
    /// <summary>
    /// Called when navigation is moving away from this view/view model.
    /// 当导航从当前视图/视图模型离开时调用。
    /// </summary>
    /// <param name="entry">The current navigation entry (Uri, parameters). 当前导航条目（Uri 与 参数）。</param>
    public void OnNavigatedFrom((Uri, object) entry);

    /// <summary>
    /// Called when navigation moves to this view/view model.
    /// 当导航到达当前视图/视图模型时调用。
    /// </summary>
    /// <param name="entry">The new navigation entry (Uri, parameters). 新的导航条目（Uri 与 参数）。</param>
    public void OnNavigatedTo((Uri, object) entry);
}
