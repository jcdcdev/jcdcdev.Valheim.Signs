using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using BepInEx;
using BepInEx.Bootstrap;
using jcdcdev.Valheim.Core;
using jcdcdev.Valheim.Core.Extensions;
using jcdcdev.Valheim.Core.RPC;
using jcdcdev.Valheim.Signs.Converters;
using jcdcdev.Valheim.Signs.Extensions;
using jcdcdev.Valheim.Signs.Models;
using jcdcdev.Valheim.Signs.RPC;
using Jotunn.Managers;
using Jotunn.Utils;

namespace jcdcdev.Valheim.Signs;

[BepInPlugin(Constants.PluginId, Constants.PluginName, VersionInfo.Version)]
[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Patch)]
public class SignsPlugin : BasePlugin<SignsPlugin>
{
    public static readonly bool IsAzuSignsInstalled = Chainloader.PluginInfos.ContainsKey("Azumatt.AzuSigns");

    private static readonly List<IAmADynamicSign> DynamicSigns = new();
    public readonly ISimpleRPC DeathLeaderboardUpdateRequest = new DeathLeaderboardUpdateRequest();
    public readonly ISimpleRPC DeathLeaderboardUpdateResponse = new DeathLeaderboardUpdateResponse();
    public readonly ISimpleRPC DeathUpdate = new DeathUpdate();
    private string LeaderboardPath => $"{ConfigBasePath}death-leaderboard.json";

    protected override string PluginId => Constants.PluginId;

    protected override void OnAwake()
    {
        DeathUpdate.Initialise(NetworkManager.Instance);
        DeathLeaderboardUpdateRequest.Initialise(NetworkManager.Instance);
        DeathLeaderboardUpdateResponse.Initialise(NetworkManager.Instance);

        AddSigns();
        EnsureLeaderboardFileExists();
    }

    private void EnsureLeaderboardFileExists()
    {
        if (File.Exists(LeaderboardPath))
        {
            return;
        }

        WriteJsonToFile(PlayerDeathLeaderBoard.Empty, LeaderboardPath);
    }

    private void AddSigns()
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
    }

    private void AddSign(IAmADynamicSign sign) => DynamicSigns.Add(sign);

    public bool TryGetSignText(Sign sign, string? input, out string? output)
    {
        output = null;
        if (input == null)
        {
            Logger.LogDebug("No token found.");
            return false;
        }

        var converter = DynamicSigns.FirstOrDefault(x => x.CanConvert(sign, input));
        if (converter == null)
        {
            Logger.LogDebug($"No converter found for {input}");
            return false;
        }

        var result = converter.GetSignText(sign, input);
        if (result == null || result.IsNullOrWhiteSpace())
        {
            Logger.LogDebug("No result found.");
            return false;
        }

        output = result;
        return true;
    }

    public bool Client_GetSignText(Sign sign, string signText, out string output)
    {
        output = signText;

        try
        {
            var originalValues = Client_GetTokenValue(signText);
            foreach (var originalValue in originalValues)
            {
                if (!TryGetSignText(sign, originalValue, out var result) || result == null)
                {
                    continue;
                }

                output = output.Replace("{{" + originalValue + "}}", result);
            }

            return output != signText;
        }
        catch (Exception ex)
        {
            Logger.LogIssue(ex, "Error getting sign text");
            return false;
        }
    }

    private IEnumerable<string> Client_GetTokenValue(string originalText)
    {
        try
        {
            var match = Constants.HandlebarRegexPattern.Matches(originalText);
            if (match.Count == 0)
            {
                return new List<string>();
            }

            var output = new List<string>();
            foreach (Match item in match)
            {
                output.Add(item.Groups[1].Value);
            }

            return output;
        }
        catch (Exception ex)
        {
            Logger.LogIssue(ex, "Error getting sign token values");
            return new List<string>();
        }
    }

    public bool Client_GetSignHoverText(Sign sign, string originalText, out string output)
    {
        Logger.LogDebug($"Sign Hover: {originalText}");
        output = string.Empty;
        var originalValues = Client_GetTokenValue(originalText);
        var values = new List<string>();
        foreach (var originalValue in originalValues)
        {
            if (!TryGetSignHoverText(sign, originalValue, out var result) || result == null)
            {
                continue;
            }

            values.Add(result);
        }

        if (values.Count == 0)
        {
            return false;
        }

        var hover = $"[<color=\"yellow\">DYNAMIC</color>] {string.Join(" ", values)}";
        output = Constants.HoverTextRegexPattern.Replace(originalText, $"{hover}");
        Logger.LogDebug($"Sign Hover: {output}");
        return output != originalText;
    }

    private bool TryGetSignHoverText(Sign sign, string? originalValue, out string? hover)
    {
        hover = null;
        if (originalValue == null)
        {
            Logger.LogDebug("No token found.");
            return false;
        }

        var converter = DynamicSigns.FirstOrDefault(x => x.CanConvert(sign, originalValue));
        if (converter == null)
        {
            Logger.LogDebug("No converter found.");
            return false;
        }

        var result = converter.GetSignHoverText(sign, originalValue);
        if (result == null || result.IsNullOrWhiteSpace())
        {
            Logger.LogDebug("No hover text found.");
            return false;
        }

        hover = result;
        return true;
    }

    public PlayerDeathLeaderBoard? Server_GetDeathLeaderboard()
    {
        if (!ZNet.instance.IsLocalOrServer())
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
            Logger.LogIssue(ex, "Error getting death leaderboard");
            return null;
        }
    }

    public PlayerDeathLeaderBoard? Client_GetOrRequestDeathLeaderboard()
    {
        if (!ZNet.instance.IsLocalOrClient())
        {
            Logger.LogWarning($"Client method called on server: {nameof(Client_GetOrRequestDeathLeaderboard)}");
            return null;
        }

        var model = GetCacheItem<PlayerDeathLeaderBoard>(Constants.CacheKeys.DeathLeaderboard);
        if (model != null)
        {
            Logger.LogDebug("Using cached death leaderboard.");
            return model;
        }

        DeathLeaderboardUpdateRequest.SendToServer(DateTime.MinValue.Ticks);
        return null;
    }

    public void Server_UpdateDeath(PlayerDeathInfo data)
    {
        try
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
            DeathLeaderboardUpdateResponse.SendAll(model);
            WriteJsonToFile(model, path);
        }
        catch (Exception ex)
        {
            Logger.LogIssue(ex, "Error updating death leaderboard");
        }
    }

    public void Client_SendDeathUpdateRequest(Player player)
    {
        if (!ZNet.instance.IsLocalOrClient())
        {
            Logger.LogWarning("InvokeDeathUpdateRequest called on server.");
            return;
        }

        DeathUpdate.SendToServer(player.GetDeathInfo());
    }
}
