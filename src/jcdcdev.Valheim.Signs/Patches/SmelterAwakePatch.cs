using HarmonyLib;

namespace jcdcdev.Valheim.Signs.Patches;

[HarmonyPatch(typeof(Smelter), "Awake")]
public static class SmelterAwakePatch
{
    private static void Postfix(Smelter __instance)
    {
        SignsPlugin.Instance.Client_AddSmelter(__instance);
    }
}
