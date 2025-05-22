using System;
using System.IO;
using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Core;

public static class JsonHelper
{
    public static string? ToJson(object? obj)
    {
        if (obj == null)
        {
            return null;
        }

        return SimpleJson.SimpleJson.SerializeObject(obj);
    }

    public static T? FromJson<T>(string? json) where T : class
    {
        if (json == null || string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        return SimpleJson.SimpleJson.DeserializeObject<T>(json);
    }

    public static T? ReadJsonFromFile<T>(string path) where T : class
    {
        try
        {
            var contents = File.ReadAllText(path);
            return FromJson<T>(contents);
        }
        catch (Exception ex)
        {
            LoggerExtensions.LogIssue(ex, $"Error reading {typeof(T).Name} from file");
            return null;
        }
    }
}
