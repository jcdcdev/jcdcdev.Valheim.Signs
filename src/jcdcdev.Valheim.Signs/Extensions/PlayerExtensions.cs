using System;

namespace jcdcdev.Valheim.Signs.Extensions;

public static class PlayerExtensions
{
    public static int GetDeathCount(this Player player)
    {
        var key = Constants.DeathsWorldKey(ZNet.m_world.m_uid);
        return player.GetCustomData(key, 0);
    }

    public static int IncrementDeathCount(this Player player)
    {
        SignsPlugin.Instance.Logger.LogDebug("Incrementing death count.");
        var worldKey = Constants.DeathsWorldKey(ZNet.m_world.m_uid);
        var worldDeaths = player.GetCustomData<int>(worldKey);
        worldDeaths++;
        player.SetCustomData(worldKey, $"{worldDeaths}");
        SignsPlugin.Instance.Logger.LogDebug($"Deaths: {worldDeaths}");
        return worldDeaths;
    }

    public static T GetCustomData<T>(this Player player, string key, T defaultValue = default!)
    {
        try
        {
            if (player.m_customData.TryGetValue(key, out var value))
            {
                SignsPlugin.Instance.Logger.LogDebug($"Getting custom data for player: {player.GetPlayerName()} with key: {key} and value: {value}");
                var output = (T)Convert.ChangeType(value, typeof(T));
                return output ?? defaultValue;
            }
        }
        catch (Exception ex)
        {
            SignsPlugin.Instance.Logger.LogError(ex);
            return defaultValue;
        }

        return defaultValue;
    }

    public static void SetCustomData(this Player player, string key, string value)
    {
        SignsPlugin.Instance.Logger.LogDebug($"Setting custom data for player: {player.GetPlayerName()} with key: {key} and value: {value}");
        player.m_customData[key] = value;
    }

    private static class Constants
    {
        private const string CustomDataKeyPrefix = "jcdcdev";
        public static string DeathsWorldKey(long worldUid) => $"{CustomDataKeyPrefix}-deaths-world-{worldUid}";
    }
}
