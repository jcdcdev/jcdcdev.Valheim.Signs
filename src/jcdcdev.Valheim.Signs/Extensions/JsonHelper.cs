using Newtonsoft.Json;

namespace jcdcdev.Valheim.Signs.Extensions;

public static class JsonHelper
{
    public static string? ToJson(object? obj)
    {
        if (obj == null)
        {
            return null;
        }

        return JsonConvert.SerializeObject(obj);
    }

    public static T? FromJson<T>(string? json) where T : class
    {
        if (json == null || string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<T>(json);
    }
}
