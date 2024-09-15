using Jotunn;

namespace jcdcdev.Valheim.Core.RPC;

public static class ZNetHelper
{
    public static bool IsLocalOrServer(this ZNet znet) => IsLocal(znet) || znet.IsServerInstance();
    public static bool IsLocalOrClient(this ZNet znet) => IsLocal(znet) || znet.IsClientInstance();
    public static bool IsLocal(this ZNet znet) => znet.IsLocalInstance();
}
