using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class ComfortSign : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => Player.m_localPlayer != null && input.InvariantEquals("comfort");

    public string? GetSignText(Sign sign, string input) => Player.m_localPlayer?.GetComfortLevel().ToString();

    public string? GetSignHoverText(Sign sign, string input) => "Comfort";
}
