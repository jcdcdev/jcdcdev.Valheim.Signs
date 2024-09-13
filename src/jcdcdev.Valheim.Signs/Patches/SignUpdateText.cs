using System;
using BepInEx;
using HarmonyLib;

// ReSharper disable InconsistentNaming

namespace jcdcdev.Valheim.Signs.Patches;

[HarmonyPatch(typeof(Sign), nameof(Sign.UpdateText))]
[HarmonyPriority(Priority.Last)]
public static class SignUpdateText
{
    public static void Postfix(Sign __instance)
    {
        SignsPlugin.Instance.Logger.LogDebug("SignUpdateText.Postfix");
        var view = __instance.m_nview;
        if (view == null || !view.IsValid())
        {
            SignsPlugin.Instance.Logger.LogWarning("SignUpdateText.Postfix: view is null or invalid");
            return;
        }

        try
        {
            view.ClaimOwnership();
            var zdo = view.GetZDO();
            var text = zdo.GetString("newText");
            if (text.IsNullOrWhiteSpace())
            {
                text = zdo.GetString("text", __instance.m_defaultText);
            }

            if (!SignsPlugin.Instance.Client_GetSignText(__instance, text, out var output))
            {
                SignsPlugin.Instance.Logger.LogDebug("SignUpdateText.Postfix: No output");
                return;
            }

            __instance.m_textWidget.text = output;
            zdo.Set("newText", output);
        }
        catch (Exception ex)
        {
            SignsPlugin.Instance.Logger.LogError(ex);
        }
    }
}
