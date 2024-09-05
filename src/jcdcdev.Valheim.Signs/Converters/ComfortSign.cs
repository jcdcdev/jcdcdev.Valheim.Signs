using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters
{
    public class ComfortSign : IAmADynamicSign
    {
        public bool CanConvert(Sign sign, string originalText) => Player.m_localPlayer != null && originalText.InvariantEquals("comfort");
        
        public string? GetSignText(Sign sign, string originalText) => Player.m_localPlayer?.GetComfortLevel().ToString();

        public string? GetSignHoverText(Sign sign, string originalText) => "Comfort";
    }
}