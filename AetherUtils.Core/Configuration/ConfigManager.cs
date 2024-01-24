using System.Diagnostics;
using System.Reflection;
using AetherUtils.Core.Attributes;
using AetherUtils.Core.Files;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AetherUtils.Core.Configuration;

/// <summary>
/// This class cannot be instantiated. A child class must be created inheriting from <see cref="ConfigManager{T}"/>.
/// Handles saving, loading, and querying a generic configuration based on class <see cref="T"/>.
/// </summary>
/// <typeparam name="T">The DTO class that represents the configuration.</typeparam>
public abstract class ConfigManager<T>(string configFilePath) : IConfig
    where T : class
{
    private IDeserializer _deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IncludeNonPublicProperties().Build();
    
    private ISerializer _serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IncludeNonPublicProperties().Build();

    protected T? CurrentConfig { get; set; }

    public string ConfigFilePath { get; set; } = configFilePath;

    public bool IsInitialized { get => CurrentConfig != null; }

    public bool Load()
    {
        try
        {
            configFilePath = FileHelper.ExpandPath(configFilePath);
            var text = File.ReadAllText(configFilePath);
            CurrentConfig = _deserializer.Deserialize<T>(text);
        } catch (Exception ex) { Debug.WriteLine(ex.StackTrace); return false; }
        
        return IsInitialized;
    }

    public bool Save()
    {
        try
        {
            configFilePath = FileHelper.ExpandPath(configFilePath);
            var serializedString = _serializer.Serialize(CurrentConfig);
            FileHelper.CreateDirectories(configFilePath);
            File.WriteAllText(configFilePath, serializedString);
            
            return FileHelper.DoesFileExist(configFilePath, false);
        } catch (Exception ex) {Debug.WriteLine(ex.StackTrace); return false; }
    }

    //TODO: not working due to nested configuration values! Example: loggingOptions -> logFileDirectory config value is inaccessible.
    public object? Get(string configName)
    {
        if (!IsInitialized) return default;
        var prop = (from property in typeof(T).GetProperties(BindingFlags.NonPublic | 
                                                             BindingFlags.Instance | BindingFlags.Public)
            from attrib in property.GetCustomAttributes(typeof(ConfigAttribute), false).Cast<ConfigAttribute>()
            where attrib.Name.Equals(configName)
            select property).FirstOrDefault();
        return prop?.GetValue(CurrentConfig);
    }

    public void Set(string configName, object? value)
    {
        if (!IsInitialized) return;
        var prop = (from property in typeof(T).GetProperties(BindingFlags.NonPublic | 
                                                             BindingFlags.Instance | BindingFlags.Public)
            from attrib in property.GetCustomAttributes(typeof(ConfigAttribute), false).Cast<ConfigAttribute>()
            where attrib.Name.Equals(configName)
            select property).FirstOrDefault();
        prop?.SetValue(CurrentConfig, value);
    }
}