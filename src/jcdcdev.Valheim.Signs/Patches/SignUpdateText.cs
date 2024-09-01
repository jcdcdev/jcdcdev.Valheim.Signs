using System;
using HarmonyLib;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace jcdcdev.Valheim.Signs.Patches
{
    [HarmonyPatch(typeof(Sign), nameof(Sign.UpdateText))]
    public static class SignUpdateText
    {
        public static void Postfix(Sign __instance, ref ZNetView ___m_nview)
        {
            var originalText = __instance.m_textWidget.text;
            try
            {
                if (!SignsPlugin.Instance.GetSignText(__instance, originalText, out var output))
                {
                    return;
                }

                ___m_nview.ClaimOwnership();
                __instance.m_textWidget.text = output;
                ___m_nview.GetZDO().Set("newText", output);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
}