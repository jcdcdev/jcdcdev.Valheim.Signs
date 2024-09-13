using System;
using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class ActualTimeSign : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => input.StartsWithInvariant("actualTime");

    public string? GetSignText(Sign sign, string input)
    {
        var format = TimeExtensions.GetTimeFormat(input);
        return TimeExtensions.GetRealTime(format, TimeZoneInfo.Local);
    }

    public string? GetSignHoverText(Sign sign, string input) => "Actual Time";
}
