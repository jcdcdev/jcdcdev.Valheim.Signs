using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class DayPercentSign : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => input.StartsWithInvariant("dayPercent");

    public string? GetSignText(Sign sign, string input)
    {
        var fraction = TimeExtensions.SmoothDayFraction;
        if (fraction is <= 0.25f or >= 0.75f)
        {
            return "-";
        }

        var percent = (fraction - 0.25f) * 100 / 0.5f;
        return $"{percent:F0}%";
    }

    public string? GetSignHoverText(Sign sign, string input) => "Day percentage";
}
