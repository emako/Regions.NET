using System;
using System.Collections.Generic;

namespace Regions.Extensions;

public static class UriExtension
{
    extension(Uri uri)
    {
        public string ToKey()
        {
            return uri.OriginalString.Contains("?")
                ? uri.OriginalString.Split('?')[0]
                : uri.OriginalString;
        }

        /// <summary>
        /// Parse query parameters from Uri to Dictionary<string, object>
        /// Supports both AbsoluteUri and RelativeUri.
        /// </summary>
        public Dictionary<string, object> ToParameters()
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
}
