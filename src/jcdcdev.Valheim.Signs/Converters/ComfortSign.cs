namespace jcdcdev.Valheim.Signs.Converters;

public class ComfortSign : SimpleSign
{
    public override bool CanConvert(Sign sign, string input) => base.CanConvert(sign, input) && Player.m_localPlayer != null;
    protected override string Tag => "comfort";

    protected override bool GetText(Sign sign, string input, out string? output)
    {
        output = "Comfort";
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = Player.m_localPlayer?.GetComfortLevel().ToString();
        return true;
    }
}
