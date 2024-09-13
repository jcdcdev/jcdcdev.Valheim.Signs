namespace jcdcdev.Valheim.Signs.Extensions;

public static class Network
{
    public static bool IsServer => ZNet.m_isServer || ZNet.IsSinglePlayer;

    public static bool IsClient => !IsServer;
}
