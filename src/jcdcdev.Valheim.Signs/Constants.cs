namespace jcdcdev.Valheim.Signs;

public static class Constants
{
    public const string PluginId = "jcdcdev.Valheim.Signs";
    public const string PluginName = "jcdcdev - Signs";
    public const string ConfigFileName = "jcdcdev.Signs.cfg";

    public const string GenericError = "<color=red>ERROR</color>";

    public static class CacheKeys
    {
        public static string DeathLeaderboard => "DeathLeaderboard";
    }

    public static class RPC
    {
        public static class Client
        {
            public const string DeathLeaderboardResponse = "DeathLeaderboardResponse";
            public const string BadRequest = "BadRequestMsg";
        }

        public static class Server
        {
            public const string DeathLeaderboardRequest = "DeathLeaderboardRequest";
            public const string DeathUpdate = "DeathUpdate";
        }
    }
}
