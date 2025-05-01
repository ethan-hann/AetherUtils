// ConfigManagerFactory.cs : AetherUtils
// Copyright (C) 2025  Ethan Hann
// 
// MIT License
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using JetBrains.Annotations;

namespace AetherUtils.Core.Configuration;

/// <summary>
///     Factory for creating and initializing configuration managers that optionally apply default values using a
///     custom <a href="https://en.wikipedia.org/wiki/Data_transfer_object">DTO</a> configuration class and/or a
///     custom <see cref="AetherUtils.Core.Configuration.ConfigManager{T}"/> config manager class.
/// </summary>
public static class ConfigManagerFactory
{
    /// <summary>
    ///     Creates and loads a config manager of the specified type, optionally applying default values.
    /// </summary>
    /// <typeparam name="TConfig">The config <a href="https://en.wikipedia.org/wiki/Data_transfer_object">DTO</a> type.</typeparam>
    /// <typeparam name="TManager">The concrete <see cref="AetherUtils.Core.Configuration.ConfigManager{T}"/> implementation type.</typeparam>
    /// <param name="path">The configuration file path.</param>
    /// <param name="applyAndSaveDefaults">If true, will save the config back to disk after loading and applying defaults.</param>
    /// <returns>An initialized config manager with defaults applied, or null if loading failed.</returns>
    [UsedImplicitly]
    public static TManager CreateAndLoad<TConfig, TManager>(string path, bool applyAndSaveDefaults = false)
        where TConfig : class, new()
        where TManager : ConfigManager<TConfig>, new()
    {
        var manager = new TManager
        {
            ConfigFilePath = path
        };

        if (!manager.ConfigExists)
        {
            if (!manager.CreateDefaultConfig())
                return manager;
        }
        else
        {
            manager.Load(applyAndSaveDefaults);
        }
        
        if (applyAndSaveDefaults)
            manager.Save();

        return manager;
    }
    
    /// <summary>
    ///     Creates and loads a config manager using the built-in <see cref="ConfigManager{T}"/> implementation, optionally applying default values.
    /// </summary>
    /// <param name="path">The configuration file path.</param>
    /// <param name="applyAndSaveDefaults">If true, will save the config back to disk after loading and applying defaults.</param>
    /// <typeparam name="TConfig">The config <a href="https://en.wikipedia.org/wiki/Data_transfer_object">DTO</a> type.</typeparam>
    /// <returns>An initialized config manager with defaults applied, or null if loading failed.</returns>
    [UsedImplicitly]
    public static ConfigManager<TConfig> CreateAndLoad<TConfig>(string path, bool applyAndSaveDefaults = false)
        where TConfig : class, new()
    {
        var manager = new ConfigManager<TConfig>(path);

        if (!manager.ConfigExists)
        {
            if (!manager.CreateDefaultConfig())
                return manager;
        }
        else
        {
            manager.Load(applyAndSaveDefaults);
        }
        
        if (applyAndSaveDefaults)
            manager.Save();

        return manager;
    }

    /// <summary>
    ///     Asynchronously creates and loads a config manager of the specified type, optionally applying defaults.
    /// </summary>
    /// <typeparam name="TConfig">The config <a href="https://en.wikipedia.org/wiki/Data_transfer_object">DTO</a> type.</typeparam>
    /// <typeparam name="TManager">The concrete <see cref="AetherUtils.Core.Configuration.ConfigManager{T}"/> implementation type.</typeparam>
    /// <param name="path">The configuration file path.</param>
    /// <param name="applyAndSaveDefaults">If true, will save the config back to disk after loading and applying defaults.</param>
    /// <returns>A task that returns an initialized config manager with defaults applied, or null if loading failed.</returns>
    [UsedImplicitly]
    public static async Task<TManager> CreateAndLoadAsync<TConfig, TManager>(string path, bool applyAndSaveDefaults = false)
        where TConfig : class, new()
        where TManager : ConfigManager<TConfig>, new()
    {
        var manager = new TManager
        {
            ConfigFilePath = path
        };

        if (!manager.ConfigExists)
        {
            if (!manager.CreateDefaultConfig())
                return manager;
        }
        else
        {
            await manager.LoadAsync(applyAndSaveDefaults);
        }
        
        if (applyAndSaveDefaults)
            await manager.SaveAsync();

        return manager;
    }
    
    /// <summary>
    ///     Asynchronously creates and loads a config manager using the built-in <see cref="ConfigManager{T}"/> implementation, optionally applying default values.
    /// </summary>
    /// <param name="path">The configuration file path.</param>
    /// <param name="applyAndSaveDefaults">If true, will save the config back to disk after loading and applying defaults.</param>
    /// <typeparam name="TConfig">The config <a href="https://en.wikipedia.org/wiki/Data_transfer_object">DTO</a> type.</typeparam>
    /// <returns>An initialized config manager with defaults applied, or null if loading failed.</returns>
    [UsedImplicitly]
    public static async Task<ConfigManager<TConfig>> CreateAndLoadAsync<TConfig>(string path, bool applyAndSaveDefaults = false)
        where TConfig : class, new()
    {
        var manager = new ConfigManager<TConfig>(path);

        if (!manager.ConfigExists)
        {
            if (!manager.CreateDefaultConfig())
                return manager;
        }
        else
        {
            await manager.LoadAsync(applyAndSaveDefaults);
        }
        
        if (applyAndSaveDefaults)
            await manager.SaveAsync();

        return manager;
    }
}
