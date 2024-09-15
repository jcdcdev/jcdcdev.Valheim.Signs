namespace jcdcdev.Valheim.Signs.Extensions;

public static class Network
{
    private static bool IsLocalInstance(this ZNet znet) => znet.IsServer() && !znet.IsDedicated();
    private static bool IsClientInstance(this ZNet znet) => !znet.IsServer() && !znet.IsDedicated();
    private static bool IsServerInstance(this ZNet znet) => znet.IsServer() && znet.IsDedicated();

    public static bool IsServer => IsLocal || ZNet.instance.IsServerInstance();
    public static bool IsClient => IsLocal || ZNet.instance.IsClientInstance();
    public static bool IsLocal => ZNet.instance.IsLocalInstance();
}
