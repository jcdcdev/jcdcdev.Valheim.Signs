using System.Linq;
using jcdcdev.Valheim.Core;
using jcdcdev.Valheim.Core.Extensions;
using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class SkillSign : SimpleSign
{
    protected override string Tag => "skill";

    protected override bool GetText(Sign sign, string input, out string? output)
    {
        output = null;
        var options = GetOptions(input);

        var skillInput = options.FirstOrDefault();
        options.Remove(skillInput);

        var skill = SkillsExtensions.Parse(skillInput);
        if (skill == Skills.SkillType.None)
        {
            output = Constants.ErrorMessage("Invalid skill");
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
                output = Constants.ErrorMessage("Too many options");
                return false;
            }

            playerId = options.First();
        }

        var player = GetPlayer(playerId);
        if (player == null)
        {
            output = Constants.ErrorMessage($"Player not found: {playerId}");
            return false;
        }

        var value = player.GetSkillLevel(skill);
        output = string.Empty;
        if (withEmoji)
        {
            output = $"{skill.ToEmoji()} ";
        }

        if (withLabel)
        {
            output += $"{skill} ";
        }

        output += $"{value:F0}";
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        var options = GetOptions(input);
        var skillInput = options.FirstOrDefault();
        var skill = SkillsExtensions.Parse(skillInput);
        if (skill == Skills.SkillType.None)
        {
            output = Constants.ErrorMessage("Invalid skill");
            return false;
        }

        output = skill.ToString();
        return true;
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
}
