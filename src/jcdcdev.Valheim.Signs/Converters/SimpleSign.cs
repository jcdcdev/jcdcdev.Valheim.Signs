using System;
using System.Collections.Generic;
using System.Linq;
using jcdcdev.Valheim.Core.Extensions;
using Jotunn;
using UnityEngine;
using Object = UnityEngine.Object;

namespace jcdcdev.Valheim.Signs.Converters;

public abstract class SimpleSign : IAmADynamicSign
{
    public virtual bool CanConvert(Sign sign, string input) => input.StartsWithInvariant(Tag);

    protected abstract string Tag { get; }

    protected List<string> GetOptions(string input, bool skipFirst = true)
    {
        var options = input
            .ToLowerInvariant()
            .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        if (skipFirst)
        {
            options.RemoveAt(0);
        }

        SignsPlugin.Instance.Logger.LogDebug($"Found {options.Count} options: {options.Select(x => x)}");
        return options;
    }

    protected abstract bool GetText(Sign sign, string input, out string? output);
    protected abstract bool GetHoverText(Sign sign, string input, out string? output);

    protected virtual bool UseCache => true;
    protected virtual TimeSpan ExpireSignTextAfter => TimeSpan.FromSeconds(30);
    protected virtual TimeSpan ExpireSignHoverTextAfter => TimeSpan.FromSeconds(30);
    private string GetCacheKey(Sign sign, string input) => $"sign:{sign.GetInstanceID()}:text:{input}".Replace(" ", "");
    private string GetHoverCacheKey(Sign sign, string input) => $"sign:{sign.GetInstanceID()}:hover:{input}".Replace(" ", "");

    public string? GetSignText(Sign sign, string input)
    {
        try
        {
            var key = GetCacheKey(sign, input);
            if (UseCache && SignsPlugin.Instance.TryGetCacheItem<string>(key, out var cachedResult))
            {
                return cachedResult;
            }

            if (!GetText(sign, input, out var result))
            {
                SignsPlugin.Instance.Logger.LogError($"Failed to get sign text: {input}");
                if (string.IsNullOrWhiteSpace(result))
                {
                    return Constants.ErrorMessage("Failed to get sign text");
                }

                return result!;
            }

            if (UseCache)
            {
                SignsPlugin.Instance.AddCacheItem(key, result, ExpireSignTextAfter);
            }

            return result;
        }
        catch (Exception ex)
        {
            SignsPlugin.Instance.Logger.LogIssue(ex, "Error in GetSignText");
            return Constants.ErrorMessage("Error");
        }
    }

    public string? GetSignHoverText(Sign sign, string input)
    {
        try
        {
            var key = GetHoverCacheKey(sign, input);
            if (SignsPlugin.Instance.TryGetCacheItem<string>(key, out var cachedResult))
            {
                return cachedResult;
            }

            if (!GetHoverText(sign, input, out var result))
            {
                SignsPlugin.Instance.Logger.LogError($"Failed to get sign hover text: {input}");
                if (string.IsNullOrWhiteSpace(result))
                {
                    return Constants.ErrorMessage("Failed to get sign hover text");
                }

                return result!;
            }

            SignsPlugin.Instance.AddCacheItem(key, result, ExpireSignHoverTextAfter);
            return result;
        }
        catch (Exception ex)
        {
            SignsPlugin.Instance.Logger.LogIssue(ex, "Error in GetSignHoverText");
            return Constants.ErrorMessage("Error");
        }
    }

    protected List<Container> GetContainers(Vector3 position, int searchRadius = 0)
    {
        try
        {
            var containers = Object.FindObjectsOfType<Container>()
                .OrderBy(x => Vector3.Distance(position, x.transform.position))
                .ToList();

            var filteredContainers = containers;
            if (searchRadius < 1)
            {
                filteredContainers = filteredContainers
                    .Take(1)
                    .ToList();
            }
            else
            {
                filteredContainers = filteredContainers
                    .Where(x => Vector3.Distance(position, x.transform.position) <= searchRadius)
                    .ToList();
            }

            return filteredContainers;
        }
        catch (Exception ex)
        {
            SignsPlugin.Instance.Logger.LogError(ex);
            return new List<Container>();
        }
    }
}
