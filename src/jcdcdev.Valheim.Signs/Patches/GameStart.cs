using System;
using HarmonyLib;

// ReSharper disable InconsistentNaming
namespace jcdcdev.Valheim.Signs.Patches;

[HarmonyPatch(typeof(Game), nameof(Game.Start))]
public static class GameStart
{
    public static void Prefix(Game __instance)
    {
        SignsPlugin.Instance.Logger.LogDebug("GameStart.Prefix");
        ZRoutedRpc.instance.Register(Constants.RPC.Server.DeathLeaderboardRequest,
            new Action<long, ZPackage>(RPC.Server.Receive_DeathLeaderboardUpdateRequest));
        ZRoutedRpc.instance.Register(Constants.RPC.Server.DeathUpdate, new Action<long, ZPackage>(RPC.Server.Receive_DeathUpdate));
        ZRoutedRpc.instance.Register(Constants.RPC.Client.DeathLeaderboardResponse,
            new Action<long, ZPackage>(RPC.Client.Receive_DeathLeaderboardUpdateResponse));
        ZRoutedRpc.instance.Register(Constants.RPC.Client.BadRequest, new Action<long, ZPackage>(RPC.Client.Receive_BadResponse));
    }
}