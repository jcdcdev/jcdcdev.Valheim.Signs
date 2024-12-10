using System;
using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class ActualTimeSign : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => input.StartsWithInvariant("actualTime");

    public string? GetSignText(Sign sign, string input)
    {
        var time = TimeExtensions.LocalNow(TimeZoneInfo.Local);
        if (input.Contains("emoji"))
        {
            return TimeExtensions.ToEmojiClock(time);
        }

        var format = TimeExtensions.GetTimeFormat(input);
        return time.ToString(format);
    }

    public string? GetSignHoverText(Sign sign, string input) => "Actual Time";
}
