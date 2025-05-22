using System;
using System.Linq;
using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class TotalDeaths : SimpleSign
{
    protected override string Tag => "totalDeaths";

    protected override bool GetText(Sign sign, string input, out string? output)
    {
        var options = GetOptions(input);
        var showLabel = !options.Any(x => x.InvariantEquals("l"));

        var leaderboard = SignsPlugin.Instance.Client_GetOrRequestDeathLeaderboard();
        if (leaderboard == null)
        {
            output = GetLoadingText();
            return false;
        }

        var total = leaderboard.Players.Sum(x => x.GetDeaths());

        if (showLabel)
        {
            output = $"Total server deaths: {total}";
        }
        else
        {
            output = $"{total}";
        }

        return true;
    }

    private string GetLoadingText()
    {
        var time = DateTime.UtcNow;
        var loadingText = new string('.', time.Second % 4);
        return $"{loadingText}";
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Total server deaths";
        return true;
    }
}
