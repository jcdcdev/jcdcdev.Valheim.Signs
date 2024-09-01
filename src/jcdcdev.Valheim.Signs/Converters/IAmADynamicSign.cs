namespace jcdcdev.Valheim.Signs.Converters
{
    public interface IAmADynamicSign
    {
        bool CanConvert(Sign sign, string originalText);
        string? GetSignText(Sign sign, string originalText);
        string? GetSignHoverText(Sign sign, string originalText);
    }
}