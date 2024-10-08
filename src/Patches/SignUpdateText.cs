﻿using System;
using BepInEx;
using HarmonyLib;
using Jotunn;

// ReSharper disable InconsistentNaming

namespace jcdcdev.Valheim.Signs.Patches;

[HarmonyPatch(typeof(Sign), nameof(Sign.UpdateText))]
[HarmonyPriority(Priority.Last)]
public static class SignUpdateText
{
    public static void Postfix(Sign __instance)
    {
        Logger.LogDebug("SignUpdateText.Postfix");
        var view = __instance.m_nview;
        if (view == null || !view.IsValid())
        {
            Logger.LogWarning("SignUpdateText.Postfix: view is null or invalid");
            return;
        }

        try
        {
            view.ClaimOwnership();
            var zdo = view.GetZDO();
            var text = GetSignText(__instance, zdo);
            if (!SignsPlugin.Instance.Client_GetSignText(__instance, text, out var output))
            {
                Logger.LogDebug("SignUpdateText.Postfix: No output");
                return;
            }

            SetSignText(__instance, zdo, output);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }

    private static void SetSignText(Sign __instance, ZDO zdo, string output)
    {
        if (SignsPlugin.IsAzuSignsInstalled)
        {
            zdo.Set("newText", output);
        }

        __instance.m_textWidget.text = output;
    }

    private static string GetSignText(Sign __instance, ZDO zdo)
    {
        var newText = zdo.GetString("newText");
        var text = zdo.GetString("text");
        var defaultText = __instance.m_defaultText;
        string output;
        if (SignsPlugin.IsAzuSignsInstalled)
        {
            output = newText.IsNullOrWhiteSpace() ? text : newText;
        }
        else
        {
            output = text.IsNullOrWhiteSpace() ? defaultText : text;
        }

        Logger.LogDebug("SignUpdateText.Postfix: NEW TEXT: " + newText);
        Logger.LogDebug("SignUpdateText.Postfix: TEXT: " + text);
        Logger.LogDebug("SignUpdateText.Postfix: DEFAULT: " + defaultText);
        Logger.LogDebug("SignUpdateText.Postfix: RESULT OUTPUT: " + output);
        return output;
    }
}
