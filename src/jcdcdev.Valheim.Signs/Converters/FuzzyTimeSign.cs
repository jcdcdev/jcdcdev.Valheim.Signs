using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters
{
    public class FuzzyTime : IAmADynamicSign
    {
        public bool CanConvert(Sign sign, string originalText) => originalText.StartsWithInvariant("fuzzyTime");

        public string? GetSignText(Sign sign, string originalText) => TimeExtensions.GetFuzzyTime();

        public string? GetSignHoverText(Sign sign, string originalText) => "Fuzzy Time";
    }
}