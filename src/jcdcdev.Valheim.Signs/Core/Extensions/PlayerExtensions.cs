using System;
using Jotunn;

namespace jcdcdev.Valheim.Core.Extensions;

public static class PlayerExtensions
{
    private const string CustomDataKeyPrefix = "jcdcdev-";

    public static T GetCustomData<T>(this Player player, string key, T defaultValue = default!)
    {
        key = $"{CustomDataKeyPrefix}{key}";
        try
        {
            if (player.m_customData.TryGetValue(key, out var value))
            {
                Logger.LogDebug($"Getting custom data for player: {player.GetPlayerName()} with key: {key} and value: {value}");
                var output = (T)Convert.ChangeType(value, typeof(T));
                return output ?? defaultValue;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            return defaultValue;
        }

        return defaultValue;
    }

    public static void SetCustomData(this Player player, string key, string value)
    {
        Logger.LogDebug($"Setting custom data for player: {player.GetPlayerName()} with key: {key} and value: {value}");
        key = $"{CustomDataKeyPrefix}{key}";
        player.m_customData[key] = value;
    }
}
