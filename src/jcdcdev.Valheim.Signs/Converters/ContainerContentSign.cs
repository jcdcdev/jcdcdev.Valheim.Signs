using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jcdcdev.Valheim.Core.Extensions;
using jcdcdev.Valheim.Signs.Extensions;

namespace jcdcdev.Valheim.Signs.Converters;

public class ContainerContentSign : SimpleSign
{
    protected override bool UseCache => true;
    protected override TimeSpan ExpireSignTextAfter => TimeSpan.FromSeconds(SignsPlugin.Instance.ItemsCacheExpireTime.Value);
    protected override string Tag => "items";

    protected override bool GetHoverText(Sign sign, string input, out string? output)
    {
        output = $"Container Count";
        return true;
    }

    protected override bool GetText(Sign sign, string input, out string? output)
    {
        var countStacks = false;
        var withLabel = true;
        var itemNames = new List<string>();
        var searchRadius = -1;
        var withTotal = false;

        var options = GetOptions(input);

        if (options.Contains("t"))
        {
            withTotal = true;
            options.Remove("t");
        }

        if (options.Contains("s"))
        {
            countStacks = true;
            options.Remove("s");
        }

        if (options.Contains("l"))
        {
            withLabel = false;
            options.Remove("l");
        }

        foreach (var option in options)
        {
            if (int.TryParse(option, out var value))
            {
                searchRadius = Math.Min(SignsPlugin.Instance.ItemsMaxRadius.Value, value);
                continue;
            }

            if (string.IsNullOrWhiteSpace(option))
            {
                continue;
            }

            itemNames.Add(option);
        }

        var results = GetContainers(sign.transform.position, searchRadius);
        if (!results.Any())
        {
            var distance = searchRadius < 1 ? "" : $" within {searchRadius} blocks";
            output = Constants.ErrorMessage($"No containers found{distance}");
            return false;
        }

        var groups = results
            .SelectMany(x => x.m_inventory.GetAllItems())
            .GroupBy(x => x.GetName());

        var sb = new StringBuilder();
        var total = 0;
        foreach (var itemGroup in groups)
        {
            if (itemNames.Any() && !itemNames.Any(x => x.InvariantEquals(itemGroup.Key)))
            {
                continue;
            }

            var count = GetCount(itemGroup, countStacks);

            if (withTotal)
            {
                total += count;
                continue;
            }

            if (withLabel)
            {
                sb.AppendLine($"{itemGroup.Key} {count}");
                continue;
            }

            sb.AppendLine($"{count}");
        }

        if (withTotal)
        {
            if (withLabel)
            {
                sb.AppendLine($"Total {total}");
            }
            else
            {
                sb.AppendLine($"{total}");
            }
        }

        output = sb.ToString();
        return true;
    }

    private static int GetCount(IGrouping<string, ItemDrop.ItemData> itemGroup, bool countStacks)
    {
        if (!itemGroup.Any())
        {
            return 0;
        }

        var sum = itemGroup.Sum(x => x.m_stack);
        if (!countStacks)
        {
            return sum;
        }

        var stackCount = itemGroup.First().GetStackSize();
        if (sum < stackCount)
        {
            return 0;
        }

        return sum / stackCount;
    }
}
