using System;

namespace jcdcdev.Valheim.Signs.Converters
{
    public class OnlineCountSign : IAmADynamicSign
    {
        public bool CanConvert(Sign sign, string originalText) => originalText.Equals("onlinecount", StringComparison.InvariantCultureIgnoreCase);

        public string? GetSignText(Sign sign, string originalText) => $"{ZNet.instance.GetPlayerList().Count}";

        public string GetSignHoverText(Sign sign, string originalText) => "Online Players";
    }    
}