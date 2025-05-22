using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class CurrentDaySign : SimpleSign
{
    protected override string Tag => "currentDay";


    protected override bool GetText(Sign sign, string input, out string? output)
    {
        output = TimeExtensions.CurrentDay.ToString();
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Current Day";
        return true;
    }
}
