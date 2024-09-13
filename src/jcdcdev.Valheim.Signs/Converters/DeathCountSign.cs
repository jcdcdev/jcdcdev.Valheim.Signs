﻿using System;
using System.Linq;
using BepInEx;
using jcdcdev.Valheim.Signs.Extensions;
using jcdcdev.Valheim.Signs.Models;

namespace jcdcdev.Valheim.Signs.Converters;

public class DeathCountSign : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => input.StartsWithInvariant("deathCount");

    public string? GetSignText(Sign sign, string input)
    {
        var playerId = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).FirstOrDefault();
        SignsPlugin.Instance.Logger.LogDebug($"PlayerId: {playerId}");

        if (playerId == null || playerId.IsNullOrWhiteSpace())
        {
            return Player.m_localPlayer?.GetDeathCount().ToString();
        }

        var leaderboard = SignsPlugin.Instance.Client_GetOrRequestDeathLeaderboard();
        if (leaderboard == null)
        {
            return "LEADERBOARD NOT FOUND";
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

        return player == null ? "PLAYER NOT FOUND" : $"{player.Deaths}";
    }

    public string? GetSignHoverText(Sign sign, string input) => "Deaths Count";
}