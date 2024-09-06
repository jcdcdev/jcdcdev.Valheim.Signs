using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters
{
    public class PlayerStaminaSign : IAmADynamicSign
    {
        public bool CanConvert(Sign sign, string originalText) => originalText.InvariantEquals("stamina");

        public string? GetSignText(Sign sign, string originalText)
        {
            var stamina = Player.m_localPlayer.m_stamina;
            var max = Player.m_localPlayer.m_maxStamina;
            return $"{stamina:F0}/{max:F0}";
        }

        public string? GetSignHoverText(Sign sign, string originalText) => "Stamina";
    }
}