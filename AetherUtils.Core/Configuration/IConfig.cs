using AetherUtils.Core.Structs;

namespace AetherUtils.Core.Configuration;

public interface IConfig
{
    public string ConfigFilePath { get; set; }
    public bool IsInitialized { get; }
    
    /// <summary>
    /// Deserialize a configuration file from disk, if it exists.
    /// </summary>
    /// <returns><c>true</c> if the file loaded successfully; <c>false</c>, otherwise.</returns>
    public bool Load();
    
    /// <summary>
    /// Serialize and save a configuration file to disk based on the current configuration.
    /// </summary>
    /// <returns><c>true</c> if the file saved successfully; <c>false</c>, otherwise.</returns>
    public bool Save();
    
    /// <summary>
    /// Get the configuration value for the named config property.
    /// </summary>
    /// <param name="configName">The name of the configuration value to get.</param>
    /// <returns>The value of the configuration property.</returns>
    public object? Get(ConfigOption option);

    /// <summary>
    /// Set a configuration value for the named config property.
    /// </summary>
    /// <param name="configName">The name of the configuration value to set.</param>
    /// <param name="value">The value to set the configuration option to.</param>
    /// <returns><c>true</c> if the value was set successfully; <c>false</c> otherwise.</returns>
    public bool Set(ConfigOption option);
}