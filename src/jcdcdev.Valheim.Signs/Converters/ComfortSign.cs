using System;

namespace jcdcdev.Valheim.Signs.Converters
{
    public class ComfortSign : IAmADynamicSign
    {
        public bool CanConvert(Sign sign, string originalText) => originalText.Equals("comfort", StringComparison.InvariantCultureIgnoreCase);

        public string? GetSignText(Sign sign, string originalText) => Player.m_localPlayer?.GetComfortLevel().ToString();

        public string? GetSignHoverText(Sign sign, string originalText) => "Comfort";
    }
}