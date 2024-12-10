using System;
using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class NightCountdownSign : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => input.StartsWithInvariant("nightCountdown");

    public string? GetSignText(Sign sign, string input)
    {
        var isSeconds = input.ToLowerInvariant().Replace("nightCountdown", string.Empty).Contains("s");
        var time = TimeExtensions.CalculateTimeLeftInDay();
        var endTime = TimeSpan.Zero;
        if (time == endTime)
        {
            return "-";
        }

        var adjusted = ConvertToRealTime(time, endTime);
        return isSeconds ? adjusted.ToString(@"mm\:ss") : adjusted.ToString(@"mm");
    }

    private static TimeSpan ConvertToRealTime(TimeSpan current, TimeSpan end)
    {
        double totalDurationSeconds = 45 * 60;
        var totalInGameDuration = end - current;
        var ratio = totalInGameDuration.TotalHours / 24.0;
        var remainingRealSeconds = totalDurationSeconds * ratio;
        var remainingTimeSpan = TimeSpan.FromSeconds(remainingRealSeconds);
        return remainingTimeSpan;
    }

    public string? GetSignHoverText(Sign sign, string input) => "Time left until night";
}
