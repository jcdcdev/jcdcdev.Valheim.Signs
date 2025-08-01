using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using jcdcdev.Valheim.Core.Extensions;
using jcdcdev.Valheim.Core.RPC;
using JetBrains.Annotations;
using Jotunn;
using Jotunn.Managers;

namespace jcdcdev.Valheim.Core;

[BepInDependency(Main.ModGuid)]
public abstract class BasePlugin<TPlugin> : BaseUnityPlugin where TPlugin : class
{
    private static TPlugin? _instance;

    private readonly IDictionary<string, CacheEntry> _cache = new ConcurrentDictionary<string, CacheEntry>();
    public readonly ISimpleRPC BadRequest = new BadRequest();

    public new readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("Signs");

    private Harmony? _harmony;
    protected abstract string PluginId { get; }
    protected string ConfigBasePath => $"{Paths.ConfigPath}{Path.DirectorySeparatorChar}{PluginId}{Path.DirectorySeparatorChar}";
    public static TPlugin Instance => _instance ?? throw new InvalidOperationException("Plugin is not loaded");

    [UsedImplicitly]
    private void Awake()
    {
        try
        {
            _instance = this as TPlugin ?? throw new InvalidOperationException($"Plugin {PluginId} is not initialised correctly");
            EnsureConfigDirectoryExists();
            BadRequest.Initialise(NetworkManager.Instance);
            OnAwake();
            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginId);
        }
        catch (Exception ex)
        {
            Logger.LogIssue(ex, "Error during plugin initialisation");
            throw;
        }
    }

    [UsedImplicitly]
    private void OnDestroy()
    {
        _instance = null;
        _harmony?.UnpatchSelf();
    }

    private void EnsureConfigDirectoryExists()
    {
        if (!Directory.Exists(ConfigBasePath))
        {
            Directory.CreateDirectory(ConfigBasePath);
        }
    }

    public void AddCacheItem<TItem>(string key, TItem? value, TimeSpan? expiry = null) where TItem : class
    {
        var entry = new CacheEntry(value, expiry.HasValue ? DateTime.UtcNow.Add(expiry.Value) : DateTime.MaxValue);
        Logger.LogDebug($"Cache entry added for key: {key}, value: {value}, expiry: {entry.ExpiryTime}");
        _cache[key] = entry;
    }

    public bool TryGetCacheItem<TItem>(string key, out TItem? item) where TItem : class
    {
        item = null;
        var result = GetCacheItem<TItem>(key);
        if (result == null)
        {
            return false;
        }

        item = result;
        return true;
    }

    public TItem? GetCacheItem<TItem>(string key, bool returnExpired = false) where TItem : class
    {
        if (!_cache.TryGetValue(key, out var entry))
        {
            Logger.LogDebug($"No cache entry found for key: {key}");
            return null;
        }

        if (entry.ExpiryTime <= DateTime.UtcNow)
        {
            Logger.LogDebug($"Cache entry for key: {key} has expired");
            _cache.Remove(key);
            return returnExpired ? entry.Value as TItem : null;
        }

        Logger.LogDebug($"Cache entry for key: {key} is valid\nexpiry {entry.ExpiryTime}\nvalue: {entry.Value}");
        return entry.Value as TItem;
    }

    protected virtual void OnAwake() { }


    protected void WriteJsonToFile(object model, string path)
    {
        try
        {
            var contents = JsonHelper.ToJson(model);
            File.WriteAllText(path, contents);
        }
        catch (Exception ex)
        {
            Logger.LogIssue(ex, $"Error writing {model.GetType().Name} to file");
        }
    }

    private record CacheEntry(object? Value, DateTime ExpiryTime)
    {
        public object? Value { get; } = Value;
        public DateTime ExpiryTime { get; } = ExpiryTime;
    }
}
