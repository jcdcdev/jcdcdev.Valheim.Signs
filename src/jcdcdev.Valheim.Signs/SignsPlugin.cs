﻿using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using System.Text.RegularExpressions;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using jcdcdev.Valheim.Signs.Converters;
using JetBrains.Annotations;
using ServerSync;

namespace jcdcdev.Valheim.Signs
{
    [BepInPlugin(PluginId, Name, VersionInfo.Version)]
    public class SignsPlugin : BaseUnityPlugin
    {
        private readonly ConfigSync _configSync = new ConfigSync(PluginId)
        {
            DisplayName = Name,
            CurrentVersion = VersionInfo.Version,
            MinimumRequiredVersion = VersionInfo.Version,
            ModRequired = true,
            IsLocked = true
        };

        public SignsPlugin()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            var signs = new List<IAmADynamicSign>();
            foreach (var type in types)
            {
                if (type.IsAbstract || type.IsInterface)
                {
                    continue;
                }

                if (!typeof(IAmADynamicSign).IsAssignableFrom(type))
                {
                    continue;
                }

                if (Activator.CreateInstance(type) is IAmADynamicSign sign)
                {
                    signs.Add(sign);
                }
            }

            _dynamicSigns = signs.ToArray();
        }

        public static readonly ManualLogSource Log = new ManualLogSource(Name);
        private readonly IAmADynamicSign[] _dynamicSigns;
        private const string Name = "jcdcdev - Signs";
        private const string PluginId = "jcdcdev.valheim.signs";
        public static SignsPlugin Instance => _instance ?? throw new InvalidOperationException("Plugin is not loaded");
        private static SignsPlugin? _instance;
        private Harmony? _harmony;

        [UsedImplicitly]
        private void Awake()
        {
            _instance = this;
            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginId);
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
            _instance = null;
            _harmony?.UnpatchSelf();
        }

        private static readonly Regex RegexPattern = new Regex(@"{{([^}}]+)}}", RegexOptions.Compiled);

        public bool GetSignText(Sign sign, string originalText, out string output)
        {
            output = string.Empty;
            var originalValue = GetTokenValue(originalText);
            if (originalValue == null)
            {
                return false;
            }

            foreach (var converter in _dynamicSigns)
            {
                if (!converter.CanConvert(sign, originalValue))
                {
                    continue;
                }

                var newValue = converter.GetSignText(sign, originalValue);
                if (newValue == null || newValue.IsNullOrWhiteSpace())
                {
                    continue;
                }

                output = RegexPattern.Replace(originalText, newValue, 1);
                break;
            }

            return output != originalText;
        }

        private static string? GetTokenValue(string originalText)
        {
            var match = RegexPattern.Match(originalText);
            var originalValue = match.Groups[1].Value;
            return match.Success ? originalValue : null;
        }

        public bool GetSignHoverText(Sign sign, string originalText, out string output)
        {
            output = string.Empty;
            var originalValue = GetTokenValue(originalText);
            if (originalValue == null)
            {
                return false;
            }

            foreach (var converter in _dynamicSigns)
            {
                if (!converter.CanConvert(sign, originalValue))
                {
                    continue;
                }

                var hover = converter.GetSignHoverText(sign, originalText);
                if (hover == null || hover.IsNullOrWhiteSpace())
                {
                    continue;
                }

                var regex = new Regex("\"([^\"]*)\"");
                hover = $"\"[<color=\"yellow\">DYNAMIC</color>] {hover}\"";
                output = regex.Replace(originalText, $"{hover}");
            }

            return output != originalText;
        }
    }
}