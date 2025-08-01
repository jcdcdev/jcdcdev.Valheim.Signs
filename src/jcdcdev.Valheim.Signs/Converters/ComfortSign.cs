using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class ComfortSign : SimpleSign
{
    protected override string Tag => "comfort";
    public override bool CanConvert(Sign sign, string input) => base.CanConvert(sign, input) && Player.m_localPlayer != null;

    protected override bool GetText(Sign sign, string input, out string? output)
    {
        output = "Comfort";
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        if (!PlayerExtensions.TryGetLocalPlayer(out var player) || player == null)
        {
            output = null;
            return false;
        }
        output = player.GetComfortLevel().ToString();
        return true;
    }
}
