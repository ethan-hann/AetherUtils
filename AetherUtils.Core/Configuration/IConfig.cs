using AetherUtils.Core.Structs;

namespace AetherUtils.Core.Configuration;

/// <summary>
/// Interface that all Configuration Manager classes must implement.
/// </summary>
public interface IConfig
{
    /// <summary>
    /// The file path to a configuration file. This path can contain Windows path variables (i.e., <c>%TEMP%</c>). They will be
    /// expanded when saving and loading.
    /// </summary>
    public string? ConfigFilePath { get; set; }

    /// <summary>
    /// Get a value indicating whether this configuration is initialized and ready to be used.
    /// </summary>
    public bool IsInitialized { get; }

    /// <summary>
    /// Get a value indicating whether the configuration file specified by <see cref="ConfigFilePath"/> exists.
    /// </summary>
    public bool ConfigExists { get; }

    /// <summary>
    /// Asynchronously deserialize a configuration file from disk, if it exists.
    /// </summary>
    /// <returns><c>true</c> if the file loaded successfully; <c>false</c>, otherwise.</returns>
    public Task<bool> LoadAsync();

    /// <summary>
    /// Deserialize a configuration file from disk, if it exists.
    /// </summary>
    /// <returns><c>true</c> if the file loaded successfully; <c>false</c>, otherwise.</returns>
    public bool Load();

    /// <summary>
    /// Asynchronously serialize and save a configuration file to disk based on the current configuration.
    /// </summary>
    /// <returns><c>true</c> if the file saved successfully; <c>false</c>, otherwise.</returns>
    public Task<bool> SaveAsync();

    /// <summary>
    /// Serialize and save a configuration file to disk based on the current configuration.
    /// </summary>
    /// <returns><c>true</c> if the file saved successfully; <c>false</c>, otherwise.</returns>
    public bool Save();
    
    /// <summary>
    /// Get the configuration value for the named config property.
    /// </summary>
    /// <param name="option">The <see cref="ConfigOption"/> defining the configuration parameters to get.</param>
    /// <returns>The value of the configuration property.</returns>
    public object? Get(ConfigOption option);

    /// <summary>
    /// Set a configuration value for the named config property.
    /// </summary>
    /// <param name="option">The <see cref="ConfigOption"/> defining the configuration parameters to set.</param>
    /// <returns><c>true</c> if the value was set successfully; <c>false</c> otherwise.</returns>
    public bool Set(ConfigOption option);
}