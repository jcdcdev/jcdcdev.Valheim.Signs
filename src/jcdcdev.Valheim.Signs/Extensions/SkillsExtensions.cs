using System;
using jcdcdev.Valheim.Core.Extensions;

namespace jcdcdev.Valheim.Signs.Extensions;

public static class SkillsExtensions
{
    public static Skills.SkillType Parse(string? input)
    {
        if (!Enum.TryParse(input, true, out Skills.SkillType skill))
        {
            if (input?.InvariantEquals("fists") ?? false)
            {
                return Skills.SkillType.Unarmed;
            }

            return Skills.SkillType.None;
        }

        return skill;
    }
}
