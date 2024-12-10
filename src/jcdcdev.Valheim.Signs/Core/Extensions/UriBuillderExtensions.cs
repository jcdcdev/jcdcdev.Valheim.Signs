using System;
using System.Collections.Generic;
using System.Linq;

namespace jcdcdev.Valheim.Core.Extensions;

public static class UriBuilderExtensions
{
    public static UriBuilder AddQuery(this UriBuilder uriBuilder, string key, string value)
    {
        var query = uriBuilder.Query;
        if (query.Length > 1)
        {
            query = query.Substring(1);
            query += $"&{key}={value}";
        }
        else
        {
            query = $"{key}={value}";
        }

        uriBuilder.Query = query;
        return uriBuilder;
    }

    public static UriBuilder AddQuery(this UriBuilder uriBuilder, Dictionary<string, string> query)
    {
        uriBuilder.Query = string.Join("&", query.Select(x => $"{x.Key}={x.Value}"));
        return uriBuilder;
    }

    public static string ToUriString(this UriBuilder uriBuilder)
    {
        if (uriBuilder.Port == 443)
        {
            uriBuilder.Port = -1;
        }

        return uriBuilder.ToString();
    }
}
