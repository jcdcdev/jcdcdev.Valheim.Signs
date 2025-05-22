using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class FuzzyTime : SimpleSign
{

    protected override string Tag => "fuzzyTime";
    protected override bool GetText(Sign sign, string input, out string? output)
    {
        output = TimeExtensions.GetFuzzyTime();
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Fuzzy Time";
        return true;
    }
}
