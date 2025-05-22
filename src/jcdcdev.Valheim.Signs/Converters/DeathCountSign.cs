using System.Linq;
using BepInEx;
using jcdcdev.Valheim.Core.Extensions;
using jcdcdev.Valheim.Signs.Extensions;
using jcdcdev.Valheim.Signs.Models;

namespace jcdcdev.Valheim.Signs.Converters;

public class DeathCountSign : SimpleSign
{
    protected override string Tag => "deathCount";


    protected override bool GetText(Sign sign, string input, out string? output)
    {
        var options = GetOptions(input);
        var playerId = options.FirstOrDefault();
        if (playerId.IsNullOrWhiteSpace())
        {
            output = Player.m_localPlayer?.GetDeathCount().ToString();
            return true;
        }

        var leaderboard = SignsPlugin.Instance.Client_GetOrRequestDeathLeaderboard();
        if (leaderboard == null)
        {
            output = Constants.ErrorMessage("Leaderboard not available");
            return false;
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

        output = player == null ? Constants.ErrorMessage("Player not found") : $"{player.GetDeaths()}";
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Deaths Count";
        return true;
    }
}
