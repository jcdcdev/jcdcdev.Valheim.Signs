using System;
using System.Linq;
using jcdcdev.Valheim.Core;
using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class SkillSign : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => input.StartsWithInvariant("skill");

    public string? GetSignText(Sign sign, string input)
    {
        var key = $"{sign.GetInstanceID()}-{input}";
        if (SignsPlugin.Instance.TryGetCacheItem<string>(key, out var cachedOutput))
        {
            SignsPlugin.Instance.Logger.LogDebug($"Using cached output for {input}: {cachedOutput}");
            return cachedOutput;
        }

        if (!TryGetSignText(input, out var output))
        {
            SignsPlugin.Instance.Logger.LogDebug($"Failed to get sign text for {input}");
            if (string.IsNullOrWhiteSpace(cachedOutput))
            {
                return output;
            }

            SignsPlugin.Instance.Logger.LogDebug($"Using cached output for {input}:\n\n{cachedOutput}");
            return output;
        }

        SignsPlugin.Instance.AddCacheItem(key, output, TimeSpan.FromMinutes(5));
        return output;
    }

    private static bool TryGetSignText(string input, out string? result)
    {
        result = null;
        var options = input
            .ToLowerInvariant()
            .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .ToList();

        var skillInput = options.FirstOrDefault();
        options.Remove(skillInput);

        var skill = GetSkill(skillInput);
        if (skill == Skills.SkillType.None)
        {
            result = Constants.ErrorMessage("Invalid skill");
            return false;
        }

        var withEmoji = false;
        if (options.Contains("emoji"))
        {
            options.Remove("emoji");
            withEmoji = true;
        }

        var withLabel = false;
        if (options.Contains("label"))
        {
            options.Remove("label");
            withLabel = true;
        }

        var playerId = Player.m_localPlayer.name;
        if (options.Any())
        {
            if (options.Count > 1)
            {
                result = Constants.ErrorMessage("Too many options");
                return false;
            }

            playerId = options.First();
        }

        var player = GetPlayer(playerId);
        if (player == null)
        {
            result = Constants.ErrorMessage($"Player not found: {playerId}");
            return false;
        }

        var value = player.GetSkillLevel(skill);
        var output = string.Empty;
        if (withEmoji)
        {
            output = $"{skill.ToEmoji()} ";
        }

        if (withLabel)
        {
            output += $"{skill} ";
        }

        output += $"{value:F0}";
        result = output;
        return true;
    }

    private static Skills.SkillType GetSkill(string? input)
    {
        if (!Enum.TryParse(input, true, out Skills.SkillType skill))
        {
            return Skills.SkillType.None;
        }

        return skill;
    }

    private static Player? GetPlayer(string playerId)
    {
        if (string.IsNullOrWhiteSpace(playerId))
        {
            SignsPlugin.Instance.Logger.LogDebug($"PlayerId is null or empty, using local player");
            return Player.m_localPlayer;
        }

        var players = Player.GetAllPlayers();

        SignsPlugin.Instance.Logger.LogDebug($"Found {players.Count} players");
        var payload = JsonHelper.ToJson(players.Select(x => new
        {
            Id = x.GetPlayerID(),
            Name = x.GetPlayerName()
        }));

        SignsPlugin.Instance.Logger.LogDebug($"Players: {payload}");

        Player? player;
        if (long.TryParse(playerId, out var id))
        {
            SignsPlugin.Instance.Logger.LogDebug($"PlayerId is long: {id}");
            player = players.FirstOrDefault(x => x.GetPlayerID() == id);
        }
        else
        {
            SignsPlugin.Instance.Logger.LogDebug($"PlayerId is string: {playerId}");
            player = players.FirstOrDefault(x => x.GetPlayerName().InvariantEquals(playerId));
        }

        return player;
    }

    public string? GetSignHoverText(Sign sign, string input)
    {
        var skill = GetSkill(input);
        return skill == Skills.SkillType.None ? Constants.DefaultHoverError : skill.ToString();
    }
}
