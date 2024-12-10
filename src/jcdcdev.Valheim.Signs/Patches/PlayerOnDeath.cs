using HarmonyLib;
using jcdcdev.Valheim.Signs.Extensions;
using Jotunn;

// ReSharper disable InconsistentNaming
namespace jcdcdev.Valheim.Signs.Patches;

[HarmonyPatch(typeof(Player), nameof(Player.OnDeath))]
public static class PlayerOnDeath
{
    public static void Postfix(Player __instance)
    {
        Logger.LogDebug("PlayerOnDeath.Postfix");
        __instance.IncrementDeathCount();
        SignsPlugin.Instance.Client_SendDeathUpdateRequest(__instance);
    }
}
