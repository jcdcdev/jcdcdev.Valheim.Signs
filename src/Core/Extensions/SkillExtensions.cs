using System;

namespace jcdcdev.Valheim.Core.Extensions;

public static class SkillExtensions
{
    public static string? ToEmoji(this Skills.SkillType type)
    {
        return type switch
        {
            Skills.SkillType.None => null,
            Skills.SkillType.Swords => "⚔️",
            Skills.SkillType.Knives => "🔪",
            Skills.SkillType.Clubs => "🏏",
            Skills.SkillType.Polearms => "🔱",
            Skills.SkillType.Spears => "🗡️",
            Skills.SkillType.Blocking => "🛡️",
            Skills.SkillType.Axes => "🪓",
            Skills.SkillType.Bows => "🏹",
            Skills.SkillType.ElementalMagic => "🔥",
            Skills.SkillType.BloodMagic => "🩸",
            Skills.SkillType.Unarmed => "🥊",
            Skills.SkillType.Pickaxes => "⛏️",
            Skills.SkillType.WoodCutting => "🪓",
            Skills.SkillType.Crossbows => "🏹",
            Skills.SkillType.Jump => "🦘",
            Skills.SkillType.Sneak => "🦥",
            Skills.SkillType.Run => "🏃",
            Skills.SkillType.Swim => "🏊",
            Skills.SkillType.Fishing => "🎣",
            Skills.SkillType.Ride => "🏇",
            Skills.SkillType.All => string.Empty,
            _ => string.Empty
        };
    }
}
