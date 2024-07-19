﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TuneLab.Base.Event;
using TuneLab.Base.Utils;

namespace TuneLab.Configs;

internal static class Settings
{
    public static readonly SettingsFile DefaultSettings = new();
    public static NotifiableProperty<string> Language { get; } = DefaultSettings.Language;
    public static NotifiableProperty<string> BackgroundImagePath { get; } = DefaultSettings.BackgroundImagePath;
    public static NotifiableProperty<double> BackgroundImageOpacity { get; } = DefaultSettings.BackgroundImageOpacity;
    public static NotifiableProperty<double> ParameterBoundaryExtension { get; } = DefaultSettings.ParameterBoundaryExtension;
    public static NotifiableProperty<string> KeySamplesPath { get; } = DefaultSettings.KeySamplesPath;
    public static NotifiableProperty<int> AutoSaveInterval { get; } = DefaultSettings.AutoSaveInterval;
    
    public static void Init(string path)
    {
        SettingsFile? settingsFile = null;
        if (File.Exists(path))
        {
            try
            {
                settingsFile = JsonSerializer.Deserialize<SettingsFile>(File.OpenRead(path));
            }
            catch (Exception ex)
            {
                Log.Error("Failed to deserialize settings: " + ex);
            }
        }

        settingsFile ??= DefaultSettings;

        Language.Value = settingsFile.Language;
        BackgroundImagePath.Value = settingsFile.BackgroundImagePath;
        ParameterBoundaryExtension.Value = settingsFile.ParameterBoundaryExtension;
        KeySamplesPath.Value = settingsFile.KeySamplesPath;
        AutoSaveInterval.Value = settingsFile.AutoSaveInterval;
    }

    public static void Save(string path)
    {
        try
        {
            var content = JsonSerializer.Serialize(new SettingsFile()
            {
                Language = Language,
                BackgroundImagePath = BackgroundImagePath,
                ParameterBoundaryExtension = ParameterBoundaryExtension,
                KeySamplesPath = KeySamplesPath,
                AutoSaveInterval = AutoSaveInterval,
            }, JsonSerializerOptions);

            File.WriteAllText(path, content);
        }
        catch (Exception ex)
        {
            Log.Error("Failed to save settings: " + ex);
        }
    }

    static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };
}
