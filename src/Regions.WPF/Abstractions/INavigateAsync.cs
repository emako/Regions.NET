using System;

namespace Regions.Abstractions;

/// <summary>
/// Represents a component capable of initiating navigation to a target.
/// 导航能力接口：表示可以发起到目标的导航操作的组件。
/// </summary>
public interface INavigateAsync
{
    /// <summary>
    /// Requests navigation to the specified target with optional parameters.
    /// 请求导航到指定目标，并可携带可选的导航参数。
    /// </summary>
    /// <param name="target">The target URI to navigate to. 要导航到的目标 URI。</param>
    /// <param name="navigationParameters">Optional navigation parameters. 可选的导航参数。</param>
    public void RequestNavigate(Uri target, object navigationParameters);
}
