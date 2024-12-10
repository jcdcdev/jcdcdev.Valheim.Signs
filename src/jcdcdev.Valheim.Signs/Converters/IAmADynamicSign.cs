namespace jcdcdev.Valheim.Signs.Converters;

public interface IAmADynamicSign
{
    bool CanConvert(Sign sign, string input);
    string? GetSignText(Sign sign, string input);
    string? GetSignHoverText(Sign sign, string input);
}
