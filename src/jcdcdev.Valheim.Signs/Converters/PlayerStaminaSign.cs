using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class PlayerStaminaSign : SimpleSign
{
    protected override string Tag => "stamina";

    protected override bool GetText(Sign sign, string input, out string? output)
    {
        if (!PlayerExtensions.TryGetLocalPlayer(out var player) || player == null)
        {
            output = null;
            return false;
        }

        var stamina = player.m_stamina;
        var max = player.m_maxStamina;
        output = $"{stamina:F0}/{max:F0}";
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Stamina";
        return true;
    }
}
