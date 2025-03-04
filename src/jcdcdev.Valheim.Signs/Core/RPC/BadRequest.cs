using System.Collections;
using Jotunn;

namespace jcdcdev.Valheim.Core.RPC;

public class BadRequest : SimpleRPC
{
    public override IEnumerator Client(long sender, ZPackage? pkg)
    {
        if (sender != ZRoutedRpc.instance.GetServerPeerID())
        {
            Logger.LogWarning("BadRequest called with invalid sender.");
            yield break;
        }

        if (pkg == null || pkg.Size() <= 0)
        {
            Logger.LogWarning("BadRequest called with no payload.");
            yield break;
        }

        var msg = pkg.ReadString();
        if (msg == "")
        {
            Logger.LogWarning("BadRequest called with empty payload.");
            yield break;
        }

        Logger.LogWarning($"BadRequest called: {msg}");
    }

    public override IEnumerator Server(long sender, ZPackage? pkg) => Noop(sender, pkg);
}
