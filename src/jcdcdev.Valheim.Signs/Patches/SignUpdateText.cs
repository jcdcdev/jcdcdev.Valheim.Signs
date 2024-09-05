using System;
using HarmonyLib;

// ReSharper disable InconsistentNaming

namespace jcdcdev.Valheim.Signs.Patches
{
    [HarmonyPatch(typeof(Sign), nameof(Sign.UpdateText))]
    public static class SignUpdateText
    {
        public static void Postfix(Sign __instance)
        {
            if (!__instance.m_nview?.IsValid() ?? true)
            {
                return;
            }

            try
            {
                if (!SignsPlugin.Instance.GetSignText(__instance, __instance.m_currentText, out var output))
                {
                    return;
                }

                __instance.m_nview.ClaimOwnership();
                __instance.m_textWidget.text = output;
                __instance.m_nview.GetZDO().Set("newText", output);
            }
            catch (Exception ex)
            {
                SignsPlugin.Log.LogError(ex);
            }
        }
    }
}