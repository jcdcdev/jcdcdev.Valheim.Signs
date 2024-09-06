using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters
{
    public class PlayerHealthSign : IAmADynamicSign
    {
        public bool CanConvert(Sign sign, string originalText) => originalText.InvariantEquals("health");

        public string? GetSignText(Sign sign, string originalText)
        {
            var health = Player.m_localPlayer.GetHealth();
            var max = Player.m_localPlayer.GetMaxHealth();
            return $"{health:F0}/{max:F0}";
        }

        public string? GetSignHoverText(Sign sign, string originalText) => "Health";
    }
}