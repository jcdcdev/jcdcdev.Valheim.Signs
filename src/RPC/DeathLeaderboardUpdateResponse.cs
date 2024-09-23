using System;
using System.Collections;
using jcdcdev.Valheim.Core;
using jcdcdev.Valheim.Core.RPC;
using jcdcdev.Valheim.Signs.Models;
using Jotunn;

namespace jcdcdev.Valheim.Signs.RPC;

public class DeathLeaderboardUpdateResponse : SimpleRPC
{
    public override IEnumerator Client(long sender, ZPackage? pkg)
    {
        if (pkg == null || pkg.Size() <= 0)
        {
            Logger.LogWarning("DeathLeaderboardResponse called with no payload.");
            yield break;
        }

        var payload = pkg.ReadString();
        if (payload == "")
        {
            Logger.LogWarning("DeathLeaderboardResponse called with empty payload.");
            yield break;
        }

        try
        {
            var model = JsonHelper.FromJson<PlayerDeathLeaderBoard>(payload);
            if (model == null || model.Updated == DateTime.MinValue)
            {
                Logger.LogWarning("DeathLeaderboardResponse called with invalid payload.");
                yield break;
            }

            SignsPlugin.Instance.AddCacheItem(Constants.CacheKeys.DeathLeaderboard, model);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }

    public override IEnumerator Server(long sender, ZPackage? pkg) => ZNet.instance.IsLocal() ? Client(sender, pkg) : Noop(sender, pkg);
}
