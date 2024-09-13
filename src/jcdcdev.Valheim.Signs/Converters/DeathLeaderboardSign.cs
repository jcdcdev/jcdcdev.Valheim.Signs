using System;
using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class DeathLeaderboardSign : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => input.StartsWithInvariant("deathBoard");

    public string? GetSignText(Sign sign, string input)
    {
        var take = 3;
        var options = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
        return model?.GetSignText(take);
    }

    public string? GetSignHoverText(Sign sign, string input) => "Deaths Leaderboard";
}
