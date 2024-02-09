namespace AetherUtils.Core.Configuration
{
    /// <summary>
    /// Represents a configuration manager for a YAML configuration file using <see cref="DefaultConfig"/>
    /// as the base configuration.
    /// </summary>
    /// <param name="configFilePath">The path to the configuration file.</param>
    public sealed class YamlConfigManager(string configFilePath) : ConfigManager<DefaultConfig>(configFilePath)
    {
        //TODO: make this save -or- load a configuration file from disk!
        public override void CreateConfig(bool saveToDisk = true)
        {
            CurrentConfig = new DefaultConfig();
            if (saveToDisk)
                SaveAsync();
        }
    }
}
