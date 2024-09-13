using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using ServerSync;

namespace jcdcdev.Valheim.Signs.Extensions;

public abstract class BasePlugin<TPlugin> : BaseUnityPlugin where TPlugin : class
{
    private static TPlugin? _instance;
    private readonly IDictionary<string, object?> Cache = new Dictionary<string, object?>();

    public new readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("Signs");

    private ConfigSync? _configSync;
    private Harmony? _harmony;
    protected abstract string PluginName { get; }
    protected abstract string PluginId { get; }
    protected abstract string ConfigFileName { get; }
    protected abstract string CurrentVersion { get; }
    protected virtual string MinimumRequiredVersion => CurrentVersion;
    protected string ConfigBasePath => $"{Paths.ConfigPath}{Path.DirectorySeparatorChar}{PluginId}{Path.DirectorySeparatorChar}";
    public static TPlugin Instance => _instance ?? throw new InvalidOperationException("Plugin is not loaded");

    [UsedImplicitly]
    private void Awake()
    {
        _configSync = new ConfigSync(PluginId)
        {
            DisplayName = PluginName,
            CurrentVersion = CurrentVersion,
            MinimumRequiredVersion = MinimumRequiredVersion,
            ModRequired = true,
            IsLocked = true
        };
        _instance = this as TPlugin ?? throw new InvalidOperationException("Plugin is not loaded");
        _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginId);
        SetupWatcher();
        OnAwake();
    }

    [UsedImplicitly]
    private void OnDestroy()
    {
        _instance = null;
        _harmony?.UnpatchSelf();
    }

    public void Client_AddCacheItem(string key, object? value) => Cache[key] = value;

    protected TItem? Client_GetCacheItem<TItem>(string key) where TItem : class
    {
        if (!Cache.TryGetValue(key, out var value))
        {
            return null;
        }

        if (value is TItem result)
        {
            return result;
        }

        return null;
    }

    private void ReadConfigValues(object sender, FileSystemEventArgs e)
    {
        var path = $"{ConfigBasePath}{ConfigFileName}";
        if (!File.Exists(path))
        {
            return;
        }

        try
        {
            Logger.LogInfo("ReadConfigValues called");
            Config.Reload();
        }
        catch
        {
            Logger.LogError($"There was an issue loading: {ConfigFileName}");
            Logger.LogError("Please check your config entries for spelling and format!");
        }
    }

    private void SetupWatcher()
    {
        if (!Directory.Exists(ConfigBasePath))
        {
            Directory.CreateDirectory(ConfigBasePath);
        }

        var fileSystemWatcher = new FileSystemWatcher(ConfigBasePath, ConfigFileName);
        fileSystemWatcher.Changed += ReadConfigValues;
        fileSystemWatcher.Created += ReadConfigValues;
        fileSystemWatcher.Renamed += ReadConfigValues;
        fileSystemWatcher.IncludeSubdirectories = true;
        fileSystemWatcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
        fileSystemWatcher.EnableRaisingEvents = true;
    }

    protected virtual void OnAwake() { }

    protected ConfigEntry<T> SyncedConfig<T>(
        string section,
        string key,
        T value,
        ConfigDescription description,
        bool synchronizedSetting = true)
    {
        var configEntry = Config.Bind(section, key, value, description);
        if (_configSync == null)
        {
            return configEntry;
        }

        var syncedConfigEntry = _configSync.AddConfigEntry(configEntry);
        syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

        return configEntry;
    }

    protected ConfigEntry<T> SyncedConfig<T>(
        string section,
        string key,
        T value,
        string description,
        bool synchronizedSetting = true) =>
        SyncedConfig(section, key, value, new ConfigDescription(description), synchronizedSetting);

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
