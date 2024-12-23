﻿using System;
using System.Linq;
using Logger = Jotunn.Logger;

namespace jcdcdev.Valheim.Core.Extensions;

public static class TimeExtensions
{
    private static readonly TimeSpan DayStartTime = new(6, 0, 0);
    private static readonly TimeSpan NightStartTime = new(18, 0, 0);

    public static TimeSpan CalculateTimeLeftInDay()
    {
        var currentDate = ServerTimeNow();
        var currentTime = currentDate.TimeOfDay;
        Logger.LogDebug($"Game Time:\n\n{currentTime}\nServer Time:\n\n{LocalNow().TimeOfDay}");

        if (currentTime < DayStartTime || currentTime >= NightStartTime)
        {
            return TimeSpan.Zero;
        }

        return NightStartTime - currentTime;
    }

    public static float SmoothDayFraction => EnvMan.instance.m_smoothDayFraction;
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

    public static DateTime ServerTimeNow()
    {
        var hour = (int)(SmoothDayFraction * 24f);
        var minute = (int)((SmoothDayFraction * 24f - hour) * 60f);
        var second = (int)(((SmoothDayFraction * 24f - hour) * 60f - minute) * 60f);

        var time = new TimeSpan(hour, minute, second);
        var date = DateTime.MinValue.AddDays(EnvMan.instance.GetCurrentDay());

        return date.Add(time);
    }

    public static DateTime LocalNow(TimeZoneInfo? timeZone = null)
    {
        timeZone ??= TimeZoneInfo.Local;
        var localNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZone);
        return localNow;
    }

    public static string ToEmojiClock(this DateTime? time)
    {
        var hour = time?.Hour ?? 0;
        var minute = time?.Minute ?? 0;
        return minute switch
        {
            >= 0 and < 30 => hour switch
            {
                0 => "🕛",
                1 => "🕐",
                2 => "🕑",
                3 => "🕒",
                4 => "🕓",
                5 => "🕔",
                6 => "🕕",
                7 => "🕖",
                8 => "🕗",
                9 => "🕘",
                10 => "🕙",
                11 => "🕚",
                12 => "🕛",
                13 => "🕐",
                14 => "🕑",
                15 => "🕒",
                16 => "🕓",
                17 => "🕔",
                18 => "🕕",
                19 => "🕖",
                20 => "🕗",
                21 => "🕘",
                22 => "🕙",
                23 => "🕚",
                _ => "🕛" // Default case
            },
            _ => hour switch
            {
                0 => "🕧",
                1 => "🕜",
                2 => "🕝",
                3 => "🕞",
                4 => "🕟",
                5 => "🕠",
                6 => "🕡",
                7 => "🕢",
                8 => "🕣",
                9 => "🕤",
                10 => "🕥",
                11 => "🕦",
                12 => "🕧",
                13 => "🕜",
                14 => "🕝",
                15 => "🕞",
                16 => "🕟",
                17 => "🕠",
                18 => "🕡",
                19 => "🕢",
                20 => "🕣",
                21 => "🕤",
                22 => "🕥",
                23 => "🕦",
                _ => "🕧" // Default case
            }
        };
    }
}
