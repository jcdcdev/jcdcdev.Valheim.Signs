using System;
using jcdcdev.Valheim.Signs.Extensions;
using jcdcdev.Valheim.Signs.Models;
using UnityEngine;

namespace jcdcdev.Valheim.Signs;

public static class RPC
{
    public static class Server
    {
        public static void Receive_DeathUpdate(long sender, ZPackage? pkg)
        {
            if (!Network.IsServer)
            {
                SignsPlugin.Instance.Logger.LogWarning($"Server method called on client: {nameof(Receive_DeathUpdate)}");
                return;
            }

            if (pkg == null)
            {
                SignsPlugin.Instance.Logger.LogWarning($"No payload received for {nameof(Receive_DeathUpdate)}");
                Send_BadRequest(sender, "Failed to update death count.");
                return;
            }

            SignsPlugin.Instance.Logger.LogInfo("DeathUpdate called.");
            var payload = pkg.ReadString();
            var data = JsonHelper.FromJson<PlayerDeathInfo>(payload);
            if (data == null)
            {
                SignsPlugin.Instance.Logger.LogWarning($"Failed to parse payload for {nameof(Receive_DeathUpdate)}");
                Send_BadRequest(sender, "Failed to update death count.");
                return;
            }

            SignsPlugin.Instance.Server_UpdateDeath(data);
        }

        public static void Receive_DeathLeaderboardUpdateRequest(long sender, ZPackage? pkg)
        {
            if (!Network.IsServer)
            {
                SignsPlugin.Instance.Logger.LogWarning($"Server method called on client: {nameof(Receive_DeathLeaderboardUpdateRequest)}");
                return;
            }

            if (pkg == null)
            {
                SignsPlugin.Instance.Logger.LogWarning($"No payload received for {nameof(Receive_DeathLeaderboardUpdateRequest)}");
                Send_BadRequest(sender, "Failed to get death leaderboard.");
                return;
            }

            var payload = pkg.ReadString();
            var lastUpdated = DateTime.MinValue;
            if (long.TryParse(payload, out var result))
            {
                lastUpdated = new DateTime(result);
            }

            try
            {
                var model = SignsPlugin.Instance.Server_GetDeathLeaderboard();
                if (model == null)
                {
                    return;
                }

                if (model.Updated <= lastUpdated)
                {
                    SignsPlugin.Instance.Logger.LogDebug(
                        $"DeathLeaderboard is up to date. Server Last Updated: {model.Updated} Client Last Updated: {lastUpdated}");
                    return;
                }

                Send_DeathLeaderboard(sender, model);
            }
            catch (Exception ex)
            {
                SignsPlugin.Instance.Logger.LogError(ex);
            }
        }

        public static void Send_DeathLeaderboard(long sender, PlayerDeathLeaderBoard model)
        {
            var response = new ZPackage();
            var payload = JsonHelper.ToJson(model);
            response.Write(payload);
            ZRoutedRpc.instance.InvokeRoutedRPC(sender, Constants.RPC.Client.DeathLeaderboardResponse, response);
        }

        private static void Send_BadRequest(long sender, string message)
        {
            if (!Network.IsServer)
            {
                SignsPlugin.Instance.Logger.LogWarning("BadRequest called on client.");
                return;
            }

            SignsPlugin.Instance.Logger.LogWarning($"BadRequest called {message}");
            var newPkg = new ZPackage();
            newPkg.Write(message);
            ZRoutedRpc.instance.InvokeRoutedRPC(sender, Constants.RPC.Client.BadRequest, newPkg);
        }
    }

    public static class Client
    {
        public static void Receive_DeathLeaderboardUpdateResponse(long sender, ZPackage? pkg)
        {
            if (!Network.IsClient)
            {
                SignsPlugin.Instance.Logger.LogWarning("DeathLeaderboardResponse called on server.");
                return;
            }

            if (sender != ZRoutedRpc.instance.GetServerPeerID() || pkg == null || pkg.Size() <= 0)
            {
                SignsPlugin.Instance.Logger.LogWarning("DeathLeaderboardResponse called with no payload.");
                return;
            }

            var payload = pkg.ReadString();
            if (payload == "")
            {
                SignsPlugin.Instance.Logger.LogWarning("DeathLeaderboardResponse called with empty payload.");
                return;
            }

            SignsPlugin.Instance.Logger.LogInfo($"DeathLeaderboardResponse called with payload {payload}");
            try
            {
                Chat.instance.AddString("Server", payload, Talker.Type.Shout);
                var model = JsonHelper.FromJson<PlayerDeathLeaderBoard>(payload);
                if (model == null || model.Updated == DateTime.MinValue)
                {
                    SignsPlugin.Instance.Logger.LogWarning("DeathLeaderboardResponse called with invalid payload.");
                    return;
                }

                SignsPlugin.Instance.Client_AddCacheItem(Constants.CacheKeys.DeathLeaderboard, model);
            }
            catch (Exception ex)
            {
                SignsPlugin.Instance.Logger.LogError(ex);
            }
        }

        public static void Receive_BadResponse(long sender, ZPackage? pkg)
        {
            if (!Network.IsClient)
            {
                SignsPlugin.Instance.Logger.LogWarning("BadRequest called on server.");
                return;
            }

            if (sender != ZRoutedRpc.instance.GetServerPeerID())
            {
                Debug.LogWarning("BadRequest called with invalid sender.");
                return;
            }

            if (pkg == null || pkg.Size() <= 0)
            {
                SignsPlugin.Instance.Logger.LogWarning("BadRequest called with no payload.");
                return;
            }

            var msg = pkg.ReadString();
            if (msg == "")
            {
                SignsPlugin.Instance.Logger.LogWarning("BadRequest called with empty payload.");
                return;
            }

            SignsPlugin.Instance.Logger.LogWarning($"BadRequest called: {msg}");
            Chat.instance.AddString("Server", "<color=\"red\">" + msg + "</color>", Talker.Type.Normal);
        }

        public static void InvokeDeathLeaderboardUpdateRequest(DateTime? lastUpdated = null)
        {
            if (!Network.IsClient)
            {
                SignsPlugin.Instance.Logger.LogWarning("InvokeDeathLeaderboardRequest called on server.");
                return;
            }

            SignsPlugin.Instance.Logger.LogDebug("Invoking InvokeDeathLeaderboardUpdateRequest");
            var payload = lastUpdated.GetValueOrDefault().Ticks;
            var pkg = new ZPackage();
            pkg.Write(payload);
            ZRoutedRpc.instance.InvokeRoutedRPC(Constants.RPC.Server.DeathLeaderboardRequest, pkg);
        }

        public static void InvokeDeathUpdateRequest(Player player)
        {
            if (!Network.IsClient)
            {
                SignsPlugin.Instance.Logger.LogWarning("InvokeDeathUpdateRequest called on server.");
                return;
            }

            var data = new PlayerDeathInfo
            {
                Deaths = player.GetDeathCount(),
                Id = player.GetPlayerID(),
                Name = player.GetPlayerName()
            };

            SignsPlugin.Instance.Logger.LogInfo($"Invoking DeathUpdate with data: {JsonHelper.ToJson(data)}");

            var pkg = new ZPackage();
            pkg.Write(JsonHelper.ToJson(data));
            ZRoutedRpc.instance.InvokeRoutedRPC(Constants.RPC.Server.DeathUpdate, pkg);
        }
    }
}