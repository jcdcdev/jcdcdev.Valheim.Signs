using System.Text.RegularExpressions;

namespace jcdcdev.Valheim.Signs;

public static class Constants
{
    public const string PluginId = "jcdcdev.Valheim.Signs";
    public const string PluginName = "jcdcdev - Signs";
    public static readonly Regex HandlebarRegexPattern = new("{{([^}}]+)}}", RegexOptions.Compiled);
    public static readonly Regex HoverTextRegexPattern = new("\"([^\"]*)\"", RegexOptions.Compiled);

    public static class CacheKeys
    {
        public static string DeathLeaderboard => "DeathLeaderboard";
    }

    public const string DefaultHoverError = "Invalid sign";

    public static string ErrorMessage(string error) => $"<color=\"red\">ERROR</color>\n{error}";
}
