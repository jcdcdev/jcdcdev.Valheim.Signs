namespace jcdcdev.Valheim.Signs.Converters;

public class PlayerHealthSign : SimpleSign
{
    protected override string Tag => "health";

    protected override bool GetText(Sign sign, string input, out string? output)
    {
        var health = Player.m_localPlayer.GetHealth();
        var max = Player.m_localPlayer.GetMaxHealth();
        output = $"{health:F0}/{max:F0}";
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Health";
        return true;
    }
}
