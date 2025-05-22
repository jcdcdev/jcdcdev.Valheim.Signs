namespace jcdcdev.Valheim.Signs.Converters;

public class OnlineCountSign : SimpleSign
{
    protected override string Tag => "onlineCount";

    protected override bool GetText(Sign sign, string input, out string? output)
    {
        output = $"{ZNet.instance.GetPlayerList().Count}";
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Online Players";
        return true;
    }
}
