using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class DayPercentSign : SimpleSign
{
    protected override string Tag => "dayPercent";


    protected override bool GetText(Sign sign, string input, out string? output)
    {
        var fraction = TimeExtensions.SmoothDayFraction;
        if (fraction is <= 0.25f or >= 0.75f)
        {
            output = "-";
            return true;
        }

        var percent = (fraction - 0.25f) * 100 / 0.5f;
        output = $"{percent:F0}%";
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Day percentage";
        return true;
    }
}
