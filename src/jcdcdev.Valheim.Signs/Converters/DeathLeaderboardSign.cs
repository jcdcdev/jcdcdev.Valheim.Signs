namespace jcdcdev.Valheim.Signs.Converters;

public class DeathLeaderboardSign : SimpleSign
{

    protected override string Tag => "deathBoard";

    protected override bool GetText(Sign sign, string input, out string? output)
    {
        var take = 3;
        var options = GetOptions(input);
        foreach (var option in options)
        {
            if (!int.TryParse(option, out var result))
            {
                continue;
            }

            take = result;
            break;
        }

        var model = SignsPlugin.Instance.Client_GetOrRequestDeathLeaderboard();
        output = model?.GetSignText(take);
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Deaths Leaderboard";
        return true;
    }
}
