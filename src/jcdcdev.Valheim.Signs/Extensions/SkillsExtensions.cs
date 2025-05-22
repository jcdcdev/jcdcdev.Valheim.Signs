using System;

namespace jcdcdev.Valheim.Signs.Extensions;

public static class SkillsExtensions
{
    public static Skills.SkillType Parse(string? input)
    {
        if (!Enum.TryParse(input, true, out Skills.SkillType skill))
        {
            return Skills.SkillType.None;
        }

        return skill;
    }
}
