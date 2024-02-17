namespace AetherUtils.Core.Configuration
{
    /// <summary>
    /// Represents a configuration manager for a YAML configuration file using <see cref="DefaultConfig"/>
    /// as the base configuration.
    /// </summary>
    /// <param name="configFilePath">The path to the configuration file.</param>
    public sealed class YamlConfigManager(string configFilePath) : ConfigManager<DefaultConfig>(configFilePath)
    {
        /// <summary>
        /// Create a new, default configuration in memory.
        /// </summary>
        public override bool CreateDefaultConfig()
        {
            CurrentConfig = new DefaultConfig();
            return IsInitialized;
        }
    }
}
