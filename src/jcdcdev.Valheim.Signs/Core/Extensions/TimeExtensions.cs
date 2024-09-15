using System;
using System.Linq;

namespace jcdcdev.Valheim.Core.Extensions;

public static class TimeExtensions
{
    private static float SmoothDayFraction => EnvMan.instance.m_smoothDayFraction;
    public static int CurrentDay => EnvMan.instance.GetCurrentDay();

    public static string GetTimeFormat(string? originalText)
    {
        if (originalText == null || string.IsNullOrWhiteSpace(originalText))
        {
            return "HH:mm";
        }

        var split = originalText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var options = split.Skip(1).ToList();
        var twelveHourClock = options.Any(x => x == "12");
        var option = options.LastOrDefault(x => x != "12");
        return option switch
        {
            "s" => twelveHourClock ? "h:mm:ss tt" : "HH:mm:ss",
            "f" => "f",
            "F" => "F",
            "g" => "g",
            "G" => "G",
            "r" => "r",
            "R" => "R",
            "U" => "U",
            _ => twelveHourClock ? "h:mm tt" : "HH:mm"
        };
    }

    public static string GetFuzzyTime()
    {
        var fuzzyOptions = new[]
        {
            "Midnight", "Early Morning", "Early Morning", "Before Dawn", "Before Dawn", "Dawn", "Dawn", "Morning", "Morning", "Late Morning",
            "Late Morning", "Midday", "Midday", "Early Afternoon", "Early Afternoon", "Afternoon", "Afternoon", "Evening", "Evening", "Night", "Night",
            "Late Night", "Late Night", "Midnight"
        };
        var index = Math.Min((int)(fuzzyOptions.Length * SmoothDayFraction), fuzzyOptions.Length - 1);
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

    public static string? GetRealTime(string? format = "HH:mm", TimeZoneInfo? timeZone = null)
    {
        timeZone ??= TimeZoneInfo.Local;
        var localNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZone);
        return localNow.ToString(format);
    }
}
