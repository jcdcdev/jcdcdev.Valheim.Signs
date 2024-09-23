using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jcdcdev.Valheim.Signs.Models;

public class PlayerDeathLeaderBoard
{
    public List<PlayerDeathInfo> Players { get; set; } = new();
    public DateTime Updated { get; set; }

    public string GetSignText(int take = int.MaxValue)
    {
        var players = Players.OrderByDescending(x => x.Deaths).Take(take).ToList();
        var sb = new StringBuilder();
        for (var i = 0; i < players.Count; i++)
        {
            var player = players[i];
            sb.AppendLine($"{i + 1}. {player.Name}: {player.Deaths}");
        }

        return sb.ToString();
    }
}
