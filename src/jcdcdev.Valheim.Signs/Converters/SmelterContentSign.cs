using System;
using System.Linq;
using System.Text;
using jcdcdev.Valheim.Core.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace jcdcdev.Valheim.Signs.Converters;

public class SmelterContentSign : IAmADynamicSign
{
    private const string Tag = "smelterContent";
    public bool CanConvert(Sign sign, string input) => input.StartsWithInvariant(Tag);

    public string? GetSignText(Sign sign, string input)
    {
        var options = input.ToLowerInvariant().Replace(Tag.ToLowerInvariant(), "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        var showLabel = options.All(x => x != "l");
        var showFuel = options.Any(x => x == "f");
        var showOre = options.Any(x => x == "o");

        if (!showFuel && !showOre)
        {
            showFuel = true;
            showOre = true;
            showLabel = true;
        }

        var dto = SignsPlugin.Instance.Client_GetClosestSmelter(sign.transform.position);
        if (dto == null)
        {
            return Constants.ErrorMessage($"No smelter found within {SignsPlugin.Instance.SmelterRadius.Value} blocks");
        }

        var smelters = Object.FindObjectsByType<Smelter>(FindObjectsSortMode.InstanceID) ?? Array.Empty<Smelter>();
        var smelter = smelters.FirstOrDefault(x => x.GetInstanceID() == dto.Id);
        if (smelter == null)
        {
            return Constants.ErrorMessage("Smelter Component not found with id: " + dto.Id);
        }

        var ore = smelter.GetQueueSize();
        var fuel = smelter.GetFuel();
        var oreType = smelter.GetQueuedOre();
        var fuelType = smelter.m_fuelItem.name;

        var sb = new StringBuilder();
        if (showFuel)
        {
            if (showLabel)
            {
                sb.Append($"{fuelType}: ");
            }

            sb.Append($"{fuel}\n");
        }

        if (showOre)
        {
            if (showLabel)
            {
                sb.Append($"{oreType}: ");
            }

            sb.Append($"{ore}\n");
        }

        return sb.ToString();
    }

    public string? GetSignHoverText(Sign sign, string input) => "Smelter Content";
}
