using HarmonyLib;
using jcdcdev.Valheim.Signs.Extensions;

// ReSharper disable InconsistentNaming
namespace jcdcdev.Valheim.Signs.Patches;

[HarmonyPatch(typeof(Player), nameof(Player.OnDeath))]
public static class PlayerOnDeath
{
    public static void Postfix(Player __instance)
    {
        SignsPlugin.Instance.Logger.LogDebug("PlayerOnDeath.Postfix");
        __instance.IncrementDeathCount();
        RPC.Client.InvokeDeathUpdateRequest(__instance);
    }
}
