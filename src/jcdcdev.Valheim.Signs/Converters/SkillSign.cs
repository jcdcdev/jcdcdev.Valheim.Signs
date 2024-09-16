using System;
using System.Linq;
using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class SkillSign : IAmADynamicSign
{
    public bool CanConvert(Sign sign, string input) => input.StartsWithInvariant("skill");

    public string? GetSignText(Sign sign, string input)
    {
        var withEmoji = input.Contains("emoji");
        var withLabel = input.Contains("label");
        var skill = GetSkill(input);
        if (skill == Skills.SkillType.None)
        {
            return Constants.ErrorMessage("Invalid skill");
        }

        var player = Player.m_localPlayer;
        var value = player.GetSkillLevel(skill);
        var output = string.Empty;
        if (withEmoji)
        {
            output = $"{skill.ToEmoji()} ";
        }
        if (withLabel)
        {
            output += $"{skill} ";
        }
        output += $"{value:F0}";
        return output;
    }

    private static Skills.SkillType GetSkill(string input)
    {
        var option = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).FirstOrDefault();
        if (!Enum.TryParse(option, true, out Skills.SkillType skill))
        {
            return Skills.SkillType.None;
        }

        return skill;
    }

    public string? GetSignHoverText(Sign sign, string input)
    {
        var skill = GetSkill(input);
        return skill == Skills.SkillType.None ? Constants.DefaultHoverError : skill.ToString();
    }
}
