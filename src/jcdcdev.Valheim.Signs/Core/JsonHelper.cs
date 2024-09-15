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
}
