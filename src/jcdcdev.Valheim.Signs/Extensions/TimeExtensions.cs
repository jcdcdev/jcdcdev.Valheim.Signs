using System;

namespace jcdcdev.Valheim.Signs.Extensions
{
    public static class TimeExtensions
    {
        private static float SmoothDayFraction => EnvMan.instance.m_smoothDayFraction;
        public static int CurrentDay => EnvMan.instance.GetCurrentDay();

        public static string GetFuzzyTime()
        {
            var fuzzyOptions = new[] { "Dawn", "Morning", "Noon", "Afternoon", "Dusk", "Night" };
            var index = Math.Min((int)(fuzzyOptions.Length * Fraction), fuzzyOptions.Length - 1);
            var fuzzyTime = fuzzyOptions[index];
            return $"{fuzzyTime}";
        }

        public static string GetCurrentTimeString(string? format = "HH:mm")
        {
            var hour = (int)(SmoothDayFraction * 24f);
            var minute = (int)((SmoothDayFraction * 24f - hour) * 60f);
            var second = (int)(((SmoothDayFraction * 24f - hour) * 60f - minute) * 60f);

            var now = DateTime.Now;
            var dateTimeNow = new DateTime(now.Year, now.Month, now.Day, hour, minute, second);
            return dateTimeNow.ToString(format);
        }
    }
}