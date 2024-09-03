using System;
using HarmonyLib;

namespace jcdcdev.Valheim.Signs.Extensions
{
    public static class TimeExtensions
    {
        private static float Fraction =>  (float)(AccessTools.Field(typeof(EnvMan), "m_smoothDayFraction")?.GetValue(EnvMan.instance) ?? 0f);

        public static string GetFuzzyTime()
        {
            var fuzzyOptions = new[] { "Dawn", "Morning", "Noon", "Afternoon", "Dusk", "Night" };
            var index = Math.Min((int)(fuzzyOptions.Length * Fraction), fuzzyOptions.Length - 1);
            var fuzzyTime = fuzzyOptions[index];
            return $"{fuzzyTime}";
        }

        public static string GetCurrentDay()
        {
            var currentDay = EnvMan.instance.GetCurrentDay();
            return $"{currentDay}";
        }

        public static string GetCurrentTimeString(string? format = "HH:mm")
        {
            var hour = (int)(Fraction * 24f);
            var minute = (int)((Fraction * 24f - hour) * 60f);
            var second = (int)(((Fraction * 24f - hour) * 60f - minute) * 60f);

            var now = DateTime.Now;
            var dateTimeNow = new DateTime(now.Year, now.Month, now.Day, hour, minute, second);
            return dateTimeNow.ToString(format);
        }
    }
}