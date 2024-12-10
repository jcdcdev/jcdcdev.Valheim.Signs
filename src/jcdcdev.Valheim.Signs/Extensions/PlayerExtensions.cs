using jcdcdev.Valheim.Core.Extensions;
using jcdcdev.Valheim.Signs.Models;
using Jotunn;

namespace jcdcdev.Valheim.Signs.Extensions;

public static class PlayerExtensions
{
    public static PlayerDeathInfo GetDeathInfo(this Player player)
    {
        var data = new PlayerDeathInfo
        {
            Deaths = player.GetDeathCount(),
            Id = player.GetPlayerID(),
            Name = player.GetPlayerName()
        };

        return data;
    }

    public static int GetDeathCount(this Player player)
    {
        var key = DeathsWorldKey(ZNet.m_world.m_uid);
        return player.GetCustomData(key, 0);
    }

    public static int IncrementDeathCount(this Player player)
    {
        Logger.LogDebug("Incrementing death count.");
        var worldKey = DeathsWorldKey(ZNet.m_world.m_uid);
        var worldDeaths = player.GetCustomData<int>(worldKey);
        worldDeaths++;
        player.SetCustomData(worldKey, $"{worldDeaths}");
        Logger.LogDebug($"Deaths: {worldDeaths}");
        return worldDeaths;
    }

    private static string DeathsWorldKey(long worldUid) => $"deaths-world-{worldUid}";
}
