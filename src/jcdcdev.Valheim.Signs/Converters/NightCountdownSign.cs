using System;
using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class NightCountdownSign : SimpleSign
{
    protected override string Tag => "nightCountdown";


    protected override bool GetText(Sign sign, string input, out string? output)
    {
        var isSeconds = input.ToLowerInvariant().Replace("nightCountdown", string.Empty).Contains("s");
        var time = TimeExtensions.CalculateTimeLeftInDay();
        var endTime = TimeSpan.Zero;
        if (time == endTime)
        {
            output = "-";
            return true;
        }

        var adjusted = ConvertToRealTime(time, endTime);
        output = isSeconds ? adjusted.ToString(@"mm\:ss") : adjusted.ToString(@"mm");
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Time left until night";
        return true;
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
}
