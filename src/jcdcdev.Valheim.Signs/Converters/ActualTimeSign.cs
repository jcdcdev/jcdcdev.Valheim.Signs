using System;
using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class ActualTimeSign : SimpleSign
{

    protected override string Tag => "actualTime";

    protected override bool GetText(Sign sign, string input, out string? output)
    {
        var time = TimeExtensions.LocalNow(TimeZoneInfo.Local);
        if (input.Contains("emoji"))
        {
            output = TimeExtensions.ToEmojiClock(time);
            return true;
        }

        var format = TimeExtensions.GetTimeFormat(input);
        output = time.ToString(format);
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Actual Time";
        return true;
    }
}
