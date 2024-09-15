using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class FuzzyTime : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => input.StartsWithInvariant("fuzzyTime");

    public string? GetSignText(Sign sign, string input) => TimeExtensions.GetFuzzyTime();

    public string? GetSignHoverText(Sign sign, string input) => "Fuzzy Time";
}
