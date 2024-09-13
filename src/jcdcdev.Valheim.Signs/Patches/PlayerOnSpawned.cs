using HarmonyLib;

// ReSharper disable once InconsistentNaming
namespace jcdcdev.Valheim.Signs.Patches;

[HarmonyPatch(typeof(Player), nameof(Player.OnSpawned))]
public class PlayerOnSpawned
{
    public static void Postfix(Player __instance)
    {
        SignsPlugin.Instance.Logger.LogDebug("PlayerOnSpawned.Postfix");
        RPC.Client.InvokeDeathUpdateRequest(__instance);
    }
}
