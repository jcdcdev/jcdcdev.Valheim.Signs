using System;
using HarmonyLib;
using Jotunn;

// ReSharper disable InconsistentNaming

namespace jcdcdev.Valheim.Signs.Patches;

[HarmonyPatch(typeof(Sign), nameof(Sign.GetHoverText))]
public static class SignGetHoverText
{
    public static void Postfix(Sign __instance, ref string __result)
    {
        Logger.LogDebug("SignGetHoverText.Postfix");
        try
        {
            string originalText;
            if (SignsPlugin.IsAzuSignsInstalled)
            {
                originalText = __result;
            }
            else
            {
                var view = __instance.m_nview;
                var zdo = view.GetZDO();
                var text = zdo.GetString("text");
                originalText = text;
            }

            if (!SignsPlugin.Instance.Client_GetSignHoverText(__instance, originalText, out var output))
            {
                Logger.LogWarning("SignGetHoverText.Postfix: No output");
                return;
            }

            __result = output;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }
}
