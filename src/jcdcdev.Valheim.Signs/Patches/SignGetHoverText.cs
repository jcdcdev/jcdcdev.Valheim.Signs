using System;
using HarmonyLib;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace jcdcdev.Valheim.Signs.Patches
{
    [HarmonyPatch(typeof(Sign), nameof(Sign.GetHoverText))]
    public static class SignGetHoverText
    {
        public static void Postfix(Sign __instance, ref string __result)
        {
            try
            {
                var originalText = __result;
                if (!SignsPlugin.Instance.GetSignHoverText(__instance, originalText, out var output))
                {
                    return;
                }

                __result = output;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
}