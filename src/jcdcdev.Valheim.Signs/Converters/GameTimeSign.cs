using System;
using System.Linq;
using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters
{
    public class GameTimeSign : IAmADynamicSign
    {
        public bool CanConvert(Sign sign, string originalText) => originalText.StartsWith("gameTime", StringComparison.InvariantCultureIgnoreCase);

        public string? GetSignText(Sign sign, string originalText)
        {
            var split = originalText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var option = split.LastOrDefault();
            var format = option?.ToLowerInvariant() switch
            {
                "12" => "h:mm tt",
                _ => "HH:mm"
            };

            return TimeExtensions.GetCurrentTimeString(format);
        }

        public string? GetSignHoverText(Sign sign, string originalText) => "Game Time";
    }
}