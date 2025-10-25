using System;
using System.Collections.Generic;

namespace Regions.Extensions;

/// <summary>
/// Extension methods for <see cref="Uri"/> used by the Regions framework.
/// Uri 扩展方法：为 Regions 框架提供便捷的 Uri 处理能力。
/// </summary>
public static class UriExtension
{
    /// <summary>
    /// Returns the navigation key part of the Uri (without query string).
    /// 返回 Uri 的导航键部分（去除查询字符串）。
    /// </summary>
    /// <param name="uri">The Uri to process. 要处理的 Uri。</param>
    /// <returns>The key string. 键字符串。</returns>
    public static string ToKey(this Uri uri)
    {
        return uri.OriginalString.Contains("?")
            ? uri.OriginalString.Split('?')[0]
            : uri.OriginalString;
    }

    /// <summary>
    /// Parse query parameters from a Uri into a dictionary.
    /// 将 Uri 中的查询参数解析为字典。
    /// Supports both AbsoluteUri and RelativeUri.
    /// 同时支持绝对与相对 Uri。
    /// </summary>
    /// <param name="uri">The Uri to parse. 要解析的 Uri。</param>
    /// <returns>A dictionary of parameters. 参数字典。</returns>
    public static Dictionary<string, object> ToParameters(this Uri uri)
    {
        Dictionary<string, object> dict = [];

        if (uri.IsAbsoluteUri)
        {
            string query = uri.Query;

            if (!string.IsNullOrEmpty(query))
            {
                query = query.TrimStart('?');
                foreach (string pair in query.Split('&'))
                {
                    string[] kv = pair.Split('=');

                    if (kv.Length == 2)
                        dict[kv[0]] = Uri.UnescapeDataString(kv[1]);
                }
            }
        }
        else
        {
            string original = uri.OriginalString;
            int idx = original.IndexOf('?');

            if (idx >= 0 && idx < original.Length - 1)
            {
                string query = original.Substring(idx + 1);

                if (!string.IsNullOrEmpty(query))
                {
                    foreach (string pair in query.Split('&'))
                    {
                        string[] kv = pair.Split('=');

                        if (kv.Length == 2)
                            dict[kv[0]] = Uri.UnescapeDataString(kv[1]);
                    }
                }
            }
        }
        return dict;
    }
}
