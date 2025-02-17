﻿using System.Linq;
using BepInEx;
using jcdcdev.Valheim.Core.Extensions;
using jcdcdev.Valheim.Signs.Extensions;
using jcdcdev.Valheim.Signs.Models;

namespace jcdcdev.Valheim.Signs.Converters;

public class DeathCountSign : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => input.StartsWithInvariant("deathCount");

    public string? GetSignText(Sign sign, string input)
    {
        var playerId = input.ToLowerInvariant().Replace("deathcount", string.Empty).Trim();
        if (playerId.IsNullOrWhiteSpace())
        {
            return Player.m_localPlayer?.GetDeathCount().ToString();
        }

        var leaderboard = SignsPlugin.Instance.Client_GetOrRequestDeathLeaderboard();
        if (leaderboard == null)
        {
            return Constants.ErrorMessage("Leaderboard not available");
        }

        PlayerDeathInfo? player;
        if (long.TryParse(playerId, out var id))
        {
            player = leaderboard.Players.FirstOrDefault(x => x.Id == id);
        }
        else
        {
            player = leaderboard.Players.FirstOrDefault(x => x.Name.InvariantEquals(playerId));
        }

        return player == null ? Constants.ErrorMessage("Player not found") : player.Deaths.ToString();
    }

    public string? GetSignHoverText(Sign sign, string input) => "Deaths Count";
}
