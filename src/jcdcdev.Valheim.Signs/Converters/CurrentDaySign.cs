using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters
{
    public class CurrentDaySign : IAmADynamicSign
    {
        public bool CanConvert(Sign sign, string originalText) => originalText.StartsWith("currentDay", System.StringComparison.InvariantCultureIgnoreCase);

        public string? GetSignText(Sign sign, string originalText) => TimeExtensions.CurrentDay.ToString();

        public string? GetSignHoverText(Sign sign, string originalText) => "Current Day";
    }
}