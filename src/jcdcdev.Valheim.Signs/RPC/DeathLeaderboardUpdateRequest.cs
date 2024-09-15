using System;
using System.Collections;
using jcdcdev.Valheim.Core.RPC;
using Jotunn;

namespace jcdcdev.Valheim.Signs.RPC;

public class DeathLeaderboardUpdateRequest : SimpleRPC
{
    public override IEnumerator Client(long sender, ZPackage? pkg) => Noop(sender, pkg);

    public override IEnumerator Server(long sender, ZPackage? pkg)
    {
        if (pkg == null)
        {
            Logger.LogWarning($"No payload received for {nameof(DeathUpdate)}");
            SignsPlugin.Instance.BadRequest.Send(sender, "No payload received for DeathUpdate");
            yield break;
        }

        var payload = pkg.ReadString();
        var lastUpdated = DateTime.MinValue;
        if (long.TryParse(payload, out var result))
        {
            lastUpdated = new DateTime(result);
        }

        try
        {
            var model = SignsPlugin.Instance.Server_GetDeathLeaderboard();
            if (model == null)
            {
                yield break;
            }

            if (model.Updated <= lastUpdated)
            {
                Logger.LogDebug($"DeathLeaderboard is up to date. Server Last Updated: {model.Updated} Client Last Updated: {lastUpdated}");
                yield break;
            }

            SignsPlugin.Instance.DeathLeaderboardUpdateResponse.Send(sender, model);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }

        yield return null;
    }
}
