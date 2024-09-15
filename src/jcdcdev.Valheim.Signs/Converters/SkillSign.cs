using System;
using System.Linq;
using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class SkillSign : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => input.StartsWithInvariant("skill");

    public string? GetSignText(Sign sign, string input)
    {
        var option = GetOption(input);
        if (string.IsNullOrWhiteSpace(option))
        {
            return Constants.ErrorMessage("No skill provided");
        }

        if (!Enum.TryParse(option, true, out Skills.SkillType skill))
        {
            return Constants.ErrorMessage("Invalid skill");
        }

        var player = Player.m_localPlayer;
        var value = player.GetSkillLevel(skill);
        return $"{value:F0}";
    }

    private static string? GetOption(string input)
    {
        var option = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).FirstOrDefault();
        return option;
    }

    public string? GetSignHoverText(Sign sign, string input) => "Skill";
}
