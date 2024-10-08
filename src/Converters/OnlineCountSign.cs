﻿using System;

namespace jcdcdev.Valheim.Signs.Converters;

public class OnlineCountSign : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => input.Equals("onlineCount", StringComparison.InvariantCultureIgnoreCase);

    public string? GetSignText(Sign sign, string input) => $"{ZNet.instance.GetPlayerList().Count}";

    public string GetSignHoverText(Sign sign, string input) => "Online Players";
}
