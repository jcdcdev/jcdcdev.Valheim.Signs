using System;
using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters
{
    public class ActualTimeSign : IAmADynamicSign
    {
        public bool CanConvert(Sign sign, string originalText) => originalText.StartsWithInvariant("actualTime");

        public string? GetSignText(Sign sign, string originalText)
        {
            var format = TimeExtensions.GetTimeFormat(originalText);
            return TimeExtensions.GetRealTime(format, TimeZoneInfo.Local);
        }

        public string? GetSignHoverText(Sign sign, string originalText) => "Actual Time";
    }
}