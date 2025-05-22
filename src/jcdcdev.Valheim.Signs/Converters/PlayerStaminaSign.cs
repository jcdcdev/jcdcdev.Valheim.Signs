namespace jcdcdev.Valheim.Signs.Converters;

public class PlayerStaminaSign : SimpleSign
{
    protected override string Tag => "stamina";

    protected override bool GetText(Sign sign, string input, out string? output)
    {
        var stamina = Player.m_localPlayer.m_stamina;
        var max = Player.m_localPlayer.m_maxStamina;
        output = $"{stamina:F0}/{max:F0}";
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Stamina";
        return true;
    }
}
