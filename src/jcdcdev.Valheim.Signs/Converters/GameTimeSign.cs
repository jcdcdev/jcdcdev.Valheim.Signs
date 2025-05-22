using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class GameTimeSign : SimpleSign
{

    protected override string Tag => "gameTime";

    protected override bool GetText(Sign sign, string input, out string? output)
    {
        var time = TimeExtensions.ServerTimeNow();
        if (input.Contains("emoji"))
        {
            output = TimeExtensions.ToEmojiClock(time);
            return true;
        }

        var format = TimeExtensions.GetTimeFormat(input);
        output = time.ToString(format);
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Game Time";
        return true;
    }
}
