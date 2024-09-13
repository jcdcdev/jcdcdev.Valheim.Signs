using System;
using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class CurrentDaySign : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => input.StartsWith("currentDay", StringComparison.InvariantCultureIgnoreCase);

    public string? GetSignText(Sign sign, string input) => TimeExtensions.CurrentDay.ToString();

    public string? GetSignHoverText(Sign sign, string input) => "Current Day";
}
