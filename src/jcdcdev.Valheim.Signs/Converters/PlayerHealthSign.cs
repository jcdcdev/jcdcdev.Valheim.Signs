using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class PlayerHealthSign : SimpleSign
{
    protected override string Tag => "health";

    protected override bool GetText(Sign sign, string input, out string? output)
    {
        if (!PlayerExtensions.TryGetLocalPlayer(out var player) || player == null)
        {
            output = null;
            return false;
        }

        var health = player.GetHealth();
        var max = player.GetMaxHealth();
        output = $"{health:F0}/{max:F0}";
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Health";
        return true;
    }
}
