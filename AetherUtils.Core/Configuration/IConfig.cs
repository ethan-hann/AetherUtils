// IConfig.cs : AetherUtils
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

using AetherUtils.Core.Structs;
using JetBrains.Annotations;

namespace AetherUtils.Core.Configuration;

/// <summary>
///     Interface that all <see cref="ConfigManager{T}" /> classes must implement.
/// </summary>
public interface IConfig
{
    /// <summary>
    ///     The file path to a configuration file. This path can contain Windows path variables (i.e., <c>%TEMP%</c>). They
    ///     will be
    ///     expanded when saving and loading.
    /// </summary>
    [UsedImplicitly]
    public string? ConfigFilePath { get; set; }

    /// <summary>
    ///     Get a value indicating whether this configuration is initialized and ready to be used.
    /// </summary>
    [UsedImplicitly]
    public bool IsInitialized { get; }

    /// <summary>
    ///     Get a value indicating whether the configuration file specified by <see cref="ConfigFilePath" /> exists.
    /// </summary>
    [UsedImplicitly]
    public bool ConfigExists { get; }

    /// <summary>
    ///     Asynchronously deserialize a configuration file from disk, if it exists.
    /// </summary>
    /// <returns><c>true</c> if the file loaded successfully; <c>false</c>, otherwise.</returns>
    [UsedImplicitly]
    public Task<bool> LoadAsync();

    /// <summary>
    ///     Deserialize a configuration file from disk, if it exists.
    /// </summary>
    /// <returns><c>true</c> if the file loaded successfully; <c>false</c>, otherwise.</returns>
    [UsedImplicitly]
    public bool Load();

    /// <summary>
    ///     Asynchronously serialize and save a configuration file to disk based on the current configuration.
    /// </summary>
    /// <returns><c>true</c> if the file saved successfully; <c>false</c>, otherwise.</returns>
    [UsedImplicitly]
    public Task<bool> SaveAsync();

    /// <summary>
    ///     Serialize and save a configuration file to disk based on the current configuration.
    /// </summary>
    /// <returns><c>true</c> if the file saved successfully; <c>false</c>, otherwise.</returns>
    [UsedImplicitly]
    public bool Save();

    /// <summary>
    ///     Get the configuration value for the named config property.
    /// </summary>
    /// <param name="option">The <see cref="ConfigOption" /> defining the configuration parameters to get.</param>
    /// <returns>The value of the configuration property.</returns>
    [UsedImplicitly]
    public object? Get(ConfigOption option);

    /// <summary>
    ///     Set a configuration value for the named config property.
    /// </summary>
    /// <param name="option">The <see cref="ConfigOption" /> defining the configuration parameters to set.</param>
    /// <returns><c>true</c> if the value was set successfully; <c>false</c> otherwise.</returns>
    [UsedImplicitly]
    public bool Set(ConfigOption option);
}