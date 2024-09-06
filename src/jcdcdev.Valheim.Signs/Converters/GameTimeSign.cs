using System;
using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters
{
    public class GameTimeSign : IAmADynamicSign
    {
        public bool CanConvert(Sign sign, string originalText) => originalText.StartsWith("gameTime", StringComparison.InvariantCultureIgnoreCase);

        public string? GetSignText(Sign sign, string originalText)
        {
            var format = TimeExtensions.GetTimeFormat(originalText);
            return TimeExtensions.GetCurrentTimeString(format);
        }

        public string? GetSignHoverText(Sign sign, string originalText) => "Game Time";
    }
}