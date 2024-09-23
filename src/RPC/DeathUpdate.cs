using System.Collections;
using jcdcdev.Valheim.Core;
using jcdcdev.Valheim.Core.RPC;
using jcdcdev.Valheim.Signs.Models;
using Jotunn;

namespace jcdcdev.Valheim.Signs.RPC;

public class DeathUpdate : SimpleRPC
{
    public override IEnumerator Client(long sender, ZPackage? pkg) => Noop(sender, pkg);

    public override IEnumerator Server(long sender, ZPackage? pkg)
    {
        if (pkg == null)
        {
            Logger.LogWarning($"No payload received for {nameof(DeathUpdate)}");
            SignsPlugin.Instance.BadRequest.Send(sender, "Failed to update death count.");
            yield break;
        }

        Logger.LogDebug("DeathUpdate called.");
        var payload = pkg.ReadString();
        var data = JsonHelper.FromJson<PlayerDeathInfo>(payload);
        if (data == null)
        {
            Logger.LogWarning($"Failed to parse payload for {nameof(DeathUpdate)}");
            SignsPlugin.Instance.BadRequest.Send(sender, "Failed to update death count.");
            yield break;
        }

        SignsPlugin.Instance.Server_UpdateDeath(data);
    }
}
