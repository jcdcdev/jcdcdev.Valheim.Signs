using System;
using HarmonyLib;

// ReSharper disable InconsistentNaming

namespace jcdcdev.Valheim.Signs.Patches;

[HarmonyPatch(typeof(Sign), nameof(Sign.GetHoverText))]
public static class SignGetHoverText
{
    public static void Postfix(Sign __instance, ref string __result)
    {
        SignsPlugin.Instance.Logger.LogDebug("SignGetHoverText.Postfix");
        try
        {
            var originalText = __result;
            if (!SignsPlugin.Instance.Client_GetSignHoverText(__instance, originalText, out var output))
            {
                SignsPlugin.Instance.Logger.LogDebug("SignGetHoverText.Postfix: No output");
                return;
            }

            __result = output;
        }
        catch (Exception ex)
        {
            SignsPlugin.Instance.Logger.LogError(ex);
        }
    }
}
