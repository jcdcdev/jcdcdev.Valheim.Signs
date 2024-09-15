using System;
using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class GameTimeSign : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => input.StartsWith("gameTime", StringComparison.InvariantCultureIgnoreCase);

    public string? GetSignText(Sign sign, string input)
    {
        var format = TimeExtensions.GetTimeFormat(input);
        return TimeExtensions.GetCurrentTimeString(format);
    }

    public string? GetSignHoverText(Sign sign, string input) => "Game Time";
}
