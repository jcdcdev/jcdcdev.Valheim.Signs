using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using BepInEx;
using BepInEx.Bootstrap;
using jcdcdev.Valheim.Signs.Converters;
using jcdcdev.Valheim.Signs.Extensions;
using jcdcdev.Valheim.Signs.Models;

namespace jcdcdev.Valheim.Signs;

[BepInPlugin(Constants.PluginId, Constants.PluginName, VersionInfo.Version)]
public class SignsPlugin : BasePlugin<SignsPlugin>
{
    public static readonly bool IsAzuSignsInstalled = Chainloader.PluginInfos.ContainsKey("Azumatt.AzuSigns");

    private static readonly List<IAmADynamicSign> DynamicSigns = new();

    private static readonly Regex RegexPattern = new(@"{{([^}}]+)}}", RegexOptions.Compiled);
    private string LeaderboardPath => $"{ConfigBasePath}death-leaderboard.json";

    protected override string PluginName => Constants.PluginName;
    protected override string PluginId => Constants.PluginId;
    protected override string ConfigFileName => Constants.ConfigFileName;
    protected override string CurrentVersion => VersionInfo.Version;

    protected override void OnAwake()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            if (type.IsAbstract || type.IsInterface)
            {
                continue;
            }

            if (!typeof(IAmADynamicSign).IsAssignableFrom(type))
            {
                continue;
            }

            if (Activator.CreateInstance(type) is IAmADynamicSign sign)
            {
                AddSign(sign);
            }
        }

        if (Network.IsServer)
        {
            if (!File.Exists(LeaderboardPath))
            {
                var model = new PlayerDeathLeaderBoard
                {
                    Players = new List<PlayerDeathInfo>(),
                    Updated = DateTime.MinValue
                };
                WriteJsonToFile(model, LeaderboardPath);
            }
        }
    }

    private void AddSign(IAmADynamicSign sign) => DynamicSigns.Add(sign);

    public bool Client_GetSignText(Sign sign, string signText, out string output)
    {
        Logger.LogDebug($"Sign Text: {signText}");
        output = signText;
        var originalValue = Client_GetTokenValue(signText);
        if (originalValue == null)
        {
            Logger.LogDebug("No token found.");
            return false;
        }

        var converter = DynamicSigns.FirstOrDefault(x => x.CanConvert(sign, originalValue));
        if (converter == null)
        {
            Logger.LogWarning($"No converter found for {originalValue}");
            return false;
        }

        var result = converter.GetSignText(sign, originalValue);
        if (result == null || result.IsNullOrWhiteSpace())
        {
            Logger.LogWarning("No result found.");
            return false;
        }

        output = RegexPattern.Replace(signText, result, 1);
        return true;
    }

    private static string? Client_GetTokenValue(string originalText)
    {
        var match = RegexPattern.Match(originalText);
        var originalValue = match.Groups[1].Value;
        return match.Success ? originalValue : null;
    }

    public bool Client_GetSignHoverText(Sign sign, string originalText, out string output)
    {
        Logger.LogDebug($"Sign Hover: {originalText}");
        output = string.Empty;
        var originalValue = Client_GetTokenValue(originalText);
        if (originalValue == null)
        {
            Logger.LogWarning("No token found.");
            return false;
        }

        var converter = DynamicSigns.FirstOrDefault(x => x.CanConvert(sign, originalValue));
        if (converter == null)
        {
            Logger.LogWarning("No converter found.");
            return false;
        }

        var hover = converter.GetSignHoverText(sign, originalText);
        if (hover == null || hover.IsNullOrWhiteSpace())
        {
            Logger.LogWarning("No hover found.");
            return false;
        }

        var regex = new Regex("\"([^\"]*)\"");
        hover = $"\"[<color=\"yellow\">DYNAMIC</color>] {hover}\"";
        output = regex.Replace(originalText, $"{hover}");
        Logger.LogDebug($"Sign Hover: {output}");
        return output != originalText;
    }

    public PlayerDeathLeaderBoard? Server_GetDeathLeaderboard()
    {
        if (!Network.IsServer)
        {
            Logger.LogWarning("GetServerDeathLeaderBoardModel called on client.");
            return null;
        }

        try
        {
            var path = LeaderboardPath;
            if (!File.Exists(path))
            {
                Logger.LogWarning("Death Leaderboard file not found.");
                return null;
            }

            var model = ReadJsonFromFile<PlayerDeathLeaderBoard>(path);
            if (model == null)
            {
                Logger.LogWarning("Death Leaderboard file is empty.");
                return null;
            }

            Logger.LogDebug($"File Based Death Leaderboard:\n\n{model.GetSignText()}");
            return model;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            return null;
        }
    }

    public PlayerDeathLeaderBoard? Client_GetOrRequestDeathLeaderboard()
    {
        if (!Network.IsClient)
        {
            Logger.LogWarning($"Client method called on server: {nameof(Client_GetOrRequestDeathLeaderboard)}");
            return null;
        }

        var model = Client_GetCacheItem<PlayerDeathLeaderBoard>(Constants.CacheKeys.DeathLeaderboard);
        if (model != null)
        {
            Logger.LogDebug("Using cached death leaderboard.");
            return model;
        }

        RPC.Client.InvokeDeathLeaderboardUpdateRequest();
        return null;
    }

    public void Server_UpdateDeath(PlayerDeathInfo data)
    {
        var path = LeaderboardPath;
        var model = ReadJsonFromFile<PlayerDeathLeaderBoard>(path);
        if (model == null)
        {
            Logger.LogWarning("Death Leaderboard file not found.");
            return;
        }

        var player = model.Players.FirstOrDefault(x => x.Id == data.Id);
        if (player == null)
        {
            Logger.LogWarning("Player not found in leaderboard.");
            model.Players.Add(data);
        }
        else
        {
            if (player.Deaths == data.Deaths)
            {
                Logger.LogDebug("Player death count is the same. No update needed.");
                return;
            }

            Logger.LogInfo($"Updating Death Count - {player.Name}: {player.Deaths} => {data.Deaths}");
            player.Deaths = data.Deaths;
        }

        model.Players = model.Players.OrderByDescending(x => x.Deaths).ToList();
        model.Updated = DateTime.UtcNow;
        RPC.Server.Send_DeathLeaderboard(ZRoutedRpc.Everybody, model);
        WriteJsonToFile(model, path);
    }
}
