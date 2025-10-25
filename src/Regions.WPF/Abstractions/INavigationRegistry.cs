using System;

namespace Regions;

/// <summary>
/// Registry for mapping navigation keys to view types.
/// 导航注册表：用于将导航键映射到视图类型。
/// </summary>
public interface INavigationRegistry
{
    /// <summary>
    /// Registers a view type with the given key.
    /// 使用指定键注册视图类型。
    /// </summary>
    /// <typeparam name="TView">The view type to register. 要注册的视图类型。</typeparam>
    /// <param name="key">The navigation key. 导航键。</param>
    public void Register<TView>(string key);

    /// <summary>
    /// Registers a view type with the given key.
    /// 使用指定键注册视图类型。
    /// </summary>
    /// <param name="type">The view type. 视图类型。</param>
    /// <param name="key">The navigation key. 导航键。</param>
    public void Register(Type type, string key);

    /// <summary>
    /// Gets the view type associated with the key.
    /// 根据导航键获取关联的视图类型。
    /// </summary>
    /// <param name="key">The navigation key. 导航键。</param>
    /// <returns>The view type or null if not found. 视图类型；若未找到则返回 null。</returns>
    public Type GetViewType(string key);
}
