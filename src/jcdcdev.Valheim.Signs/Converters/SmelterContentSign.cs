using System;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace jcdcdev.Valheim.Signs.Converters;

public class SmelterContentSign : SimpleSign
{
    protected override TimeSpan ExpireSignTextAfter => TimeSpan.FromSeconds(SignsPlugin.Instance.SmelterCacheExpireTime.Value);

    protected override string Tag => "smelterContent";
    protected override bool UseCache => true;

    protected override bool GetText(Sign sign, string input, out string? output)
    {
        var options = GetOptions(input);
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
            output = Constants.ErrorMessage($"No smelter found within {SignsPlugin.Instance.SmelterRadius.Value} blocks");
            return false;
        }

        var smelters = Object.FindObjectsByType<Smelter>(FindObjectsSortMode.InstanceID) ?? Array.Empty<Smelter>();
        var smelter = smelters.FirstOrDefault(x => x.GetInstanceID() == dto.Id);
        if (smelter == null)
        {
            output = Constants.ErrorMessage("Smelter Component not found with id: " + dto.Id);
            return false;
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

        output = sb.ToString();
        return true;
    }

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = "Smelter Content";
        return true;
    }
}
