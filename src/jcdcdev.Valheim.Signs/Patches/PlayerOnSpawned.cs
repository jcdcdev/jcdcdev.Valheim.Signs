using HarmonyLib;
using Jotunn;

// ReSharper disable once InconsistentNaming
namespace jcdcdev.Valheim.Signs.Patches;

[HarmonyPatch(typeof(Player), nameof(Player.OnSpawned))]
public class PlayerOnSpawned
{
    public static void Postfix(Player __instance)
    {
        Logger.LogDebug("PlayerOnSpawned.Postfix");
        SignsPlugin.Instance.Client_SendDeathUpdateRequest(__instance);
    }
}
