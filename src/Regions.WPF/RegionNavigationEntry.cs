using System;

namespace Regions;

/// <summary>
/// Represents a navigation entry with a URI and optional parameters.
/// 导航条目：表示包含 URI 与可选参数的导航项。
/// </summary>
/// <remarks>
/// Inherits from Tuple for backward compatibility and provides convenient conversions.
/// 继承自 Tuple 以保持向后兼容，并提供便捷的转换运算符。
/// </remarks>
public class RegionNavigationEntry(Uri uri, object parameters) : Tuple<Uri, object>(uri, parameters)
{
    /// <summary>
    /// Gets the navigation target URI.
    /// 获取导航目标 URI。
    /// </summary>
    public Uri Uri => Item1;

    /// <summary>
    /// Gets the navigation parameters.
    /// 获取导航参数。
    /// </summary>
    public object Parameters => Item2;

    /// <summary>
    /// Implicit conversion from tuple to RegionNavigationEntry.
    /// 从元组隐式转换为 RegionNavigationEntry。
    /// </summary>
    public static implicit operator RegionNavigationEntry((Uri uri, object parameters) value)
        => new(value.uri, value.parameters);

    /// <summary>
    /// Implicit conversion from RegionNavigationEntry to tuple.
    /// 从 RegionNavigationEntry 隐式转换为元组。
    /// </summary>
    public static implicit operator (Uri, object)(RegionNavigationEntry entry)
        => (entry.Uri, entry.Parameters);

    /// <summary>
    /// Equality operator comparing RegionNavigationEntry with tuple.
    /// 判断 RegionNavigationEntry 与元组是否相等的运算符。
    /// </summary>
    public static bool operator ==(RegionNavigationEntry left, (Uri, object) right)
        => left is not null && Equals(left.Uri, right.Item1) && Equals(left.Parameters, right.Item2);

    /// <summary>
    /// Inequality operator comparing RegionNavigationEntry with tuple.
    /// 判断 RegionNavigationEntry 与元组是否不相等的运算符。
    /// </summary>
    public static bool operator !=(RegionNavigationEntry left, (Uri, object) right)
        => !(left == right);

    /// <inheritdoc />
    public override bool Equals(object obj)
        => obj is RegionNavigationEntry other && Equals(Uri, other.Uri) && Equals(Parameters, other.Parameters);

    /// <inheritdoc />
    public override int GetHashCode()
    {
#if NET5_0_OR_GREATER
        return HashCode.Combine(Uri, Parameters);
#else
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + (Uri?.GetHashCode() ?? 0);
            hash = hash * 31 + (Parameters?.GetHashCode() ?? 0);
            return hash;
        }
#endif
    }
}
