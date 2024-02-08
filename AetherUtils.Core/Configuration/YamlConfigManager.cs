namespace AetherUtils.Core.Configuration
{
    /// <summary>
    /// Represents a configuration manager for a YAML configuration file using <see cref="DefaultConfig"/> as the base configuration.
    /// </summary>
    /// <param name="configFilePath">The path to the configuration file.</param>
    public class YamlConfigManager(string configFilePath) : ConfigManager<DefaultConfig>(configFilePath)
    {
        public override void CreateConfig(bool saveToDisk = true)
        {
            CurrentConfig = new DefaultConfig();
            if (saveToDisk)
                SaveAsync();
        }
    }
}
