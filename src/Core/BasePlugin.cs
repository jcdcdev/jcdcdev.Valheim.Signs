using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using jcdcdev.Valheim.Core.RPC;
using JetBrains.Annotations;
using Jotunn;
using Jotunn.Managers;

namespace jcdcdev.Valheim.Core;

[BepInDependency(Main.ModGuid)]
public abstract class BasePlugin<TPlugin> : BaseUnityPlugin where TPlugin : class
{
    private static TPlugin? _instance;
    private readonly IDictionary<string, object?> _cache = new Dictionary<string, object?>();
    public readonly ISimpleRPC BadRequest = new BadRequest();

    public new readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("Signs");

    private Harmony? _harmony;
    protected abstract string PluginId { get; }
    protected string ConfigBasePath => $"{Paths.ConfigPath}{Path.DirectorySeparatorChar}{PluginId}{Path.DirectorySeparatorChar}";
    public static TPlugin Instance => _instance ?? throw new InvalidOperationException("Plugin is not loaded");

    [UsedImplicitly]
    private void Awake()
    {
        _instance = this as TPlugin ?? throw new InvalidOperationException($"Plugin {PluginId} is not initialised correctly");
        EnsureConfigDirectoryExists();
        BadRequest.Initialise(NetworkManager.Instance);
        OnAwake();
        _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginId);
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

    public void AddCacheItem(string key, object? value) => _cache[key] = value;

    protected TItem? GetCacheItem<TItem>(string key) where TItem : class
    {
        if (!_cache.TryGetValue(key, out var value))
        {
            return null;
        }

        if (value is TItem result)
        {
            return result;
        }

        return null;
    }

    protected virtual void OnAwake() { }

    protected T? ReadJsonFromFile<T>(string path) where T : class
    {
        try
        {
            var contents = File.ReadAllText(path);
            return JsonHelper.FromJson<T>(contents);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            return null;
        }
    }

    protected void WriteJsonToFile(object model, string path)
    {
        try
        {
            var contents = JsonHelper.ToJson(model);
            File.WriteAllText(path, contents);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }
}
