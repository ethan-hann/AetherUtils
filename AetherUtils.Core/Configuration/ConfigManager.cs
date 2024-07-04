using AetherUtils.Core.Attributes;
using AetherUtils.Core.Files;
using AetherUtils.Core.Reflection;
using AetherUtils.Core.Structs;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AetherUtils.Core.Configuration;

/// <summary>
/// Provides methods for saving, loading, and querying a generic configuration.
/// This class cannot be instantiated. A child class must be created inheriting from this class.
/// </summary>
/// <remarks>
/// The custom class should have its properties related to configuration marked with a <see cref="ConfigAttribute"/>.
/// These properties are the only ones which will be serialized and de-serialized from disk.
/// </remarks>
/// <typeparam name="T">The DTO class that represents the configuration.</typeparam>
public abstract class ConfigManager<T>(string configFilePath) : IConfig
    where T : class
{
    private readonly IDeserializer _deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IncludeNonPublicProperties().Build();

    private readonly ISerializer _serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IncludeNonPublicProperties().Build();

    /// <summary>
    /// Get or set the current configuration.
    /// </summary>
    protected T? CurrentConfig { get; set; }

    /// <summary>
    /// Get or set the path to the configuration file on disk.
    /// </summary>
    public string? ConfigFilePath { get; set; } = configFilePath;

    /// <summary>
    /// Get a value indicating whether the configuration has been initialized.
    /// </summary>
    public bool IsInitialized => CurrentConfig != null;

    /// <summary>
    /// Get a value indicating whether the configuration file exists on disk.
    /// </summary>
    public bool ConfigExists => ConfigFilePath != null && FileHelper.DoesFileExist(ConfigFilePath);

    /// <summary>
    /// Create the default configuration.
    /// </summary>
    public abstract bool CreateDefaultConfig();

    /// <summary>
    /// Asynchronously load a configuration file from disk based on the <see cref="ConfigFilePath"/>.
    /// </summary>
    /// <returns><c>true</c> if the config loaded successfully; <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentException">If <see cref="ConfigFilePath"/> is <c>null</c> or empty.</exception>
    /// <exception cref="FileNotFoundException">If the configuration file specified by <see cref="ConfigFilePath"/>
    /// was not found on disk.</exception>
    public Task<bool> LoadAsync()
    {
        ArgumentException.ThrowIfNullOrEmpty(ConfigFilePath);
        
        var filePath = FileHelper.ExpandPath(ConfigFilePath);
        
        if (!FileHelper.DoesFileExist(filePath, false))
            throw new FileNotFoundException("Configuration file not found.", filePath);

        var text = FileHelper.OpenFileAsync(filePath, false);
        CurrentConfig = _deserializer.Deserialize<T>(text.Result);
        ConfigFilePath = filePath;
        
        return Task.FromResult(IsInitialized);
    }
    
    /// <summary>
    /// Load a configuration file from disk based on the <see cref="ConfigFilePath"/>.
    /// </summary>
    /// <returns><c>true</c> if the config loaded successfully; <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentException">If <see cref="ConfigFilePath"/> is <c>null</c> or empty.</exception>
    /// <exception cref="FileNotFoundException"></exception>
    public bool Load()
    {
        ArgumentException.ThrowIfNullOrEmpty(ConfigFilePath);
        
        var filePath = FileHelper.ExpandPath(ConfigFilePath);
        
        if (!FileHelper.DoesFileExist(filePath, false))
            throw new FileNotFoundException("Configuration file not found.", filePath);

        var text = FileHelper.OpenFile(filePath, false);
        CurrentConfig = _deserializer.Deserialize<T>(text);
        ConfigFilePath = filePath;

        return IsInitialized;
    }

    /// <summary>
    /// Asynchronously save a configuration file to disk based on the <see cref="CurrentConfig"/> and the <see cref="ConfigFilePath"/>.
    /// </summary>
    /// <returns><c>true</c> if the config saved successfully; <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentException">If <see cref="ConfigFilePath"/> is <c>null</c> or empty.</exception>
    public Task<bool> SaveAsync()
    {
        ArgumentException.ThrowIfNullOrEmpty(ConfigFilePath);
        
        var filePath = FileHelper.ExpandPath(ConfigFilePath);
        
        var serializedString = _serializer.Serialize(CurrentConfig);
        FileHelper.CreateDirectories(filePath);
        FileHelper.SaveFileAsync(filePath, serializedString, false);

        var fileExists = FileHelper.DoesFileExist(filePath);
        
        if (fileExists)
            ConfigFilePath = filePath;
        
        return Task.FromResult(fileExists);
    }

    /// <summary>
    /// Save a configuration file to disk based on the <see cref="CurrentConfig"/> and the <see cref="ConfigFilePath"/>.
    /// </summary>
    /// <returns><c>true</c> if the config saved successfully; <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentException">If <see cref="ConfigFilePath"/> is <c>null</c> or empty.</exception>
    public bool Save()
    {
        ArgumentException.ThrowIfNullOrEmpty(ConfigFilePath);

        var filePath = FileHelper.ExpandPath(ConfigFilePath);
        
        var serializedString = _serializer.Serialize(CurrentConfig);
        FileHelper.CreateDirectories(filePath);
        FileHelper.SaveFile(filePath, serializedString, false);

        var fileExists = FileHelper.DoesFileExist(filePath);
        
        if (fileExists)
            ConfigFilePath = filePath;

        return fileExists;
    }

    /// <summary>
    /// Get the current configuration as an object.
    /// </summary>
    /// <returns>The current configuration or <c>null</c> if not initialized.</returns>
    public T? GetConfig() => CurrentConfig;

    /// <summary>
    /// Get a configuration value specified by the configuration <paramref name="option"/>.
    /// </summary>
    /// <param name="option">The <see cref="ConfigOption"/> containing information about the value to get.</param>
    /// <returns>The configuration value or <c>null</c> if the value did not exist.</returns>
    public object? Get(ConfigOption option)
    {
        if (!IsInitialized) return false;
        if (CurrentConfig == null) return false;

        foreach (var result in Reflect.GetAttributesRecurse<ConfigAttribute>(CurrentConfig, CurrentConfig.GetType()))
        {
            var attrib = result.Property.GetCustomAttribute<ConfigAttribute>();
            if (attrib == null) continue;
            
            if (!attrib.Name.Equals(option.Name)) continue;
            
            if (!option.ArrayIndexExists)
                return result.Property.GetValue(result.Instance);
            
            var currentVal = result.Property.GetValue(result.Instance);
            if (currentVal == null || !Reflect.IsList(currentVal.GetType())) continue;
            
            try
            {
                var currentList = currentVal as IList;
                return currentList?[option.ArrayIndexer];
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        return null;
    }

    /// <summary>
    /// Get a configuration value specified by <paramref name="configName"/>.
    /// </summary>
    /// <param name="configName">The name of the configuration to get.</param>
    /// <returns>The configuration value or <c>null</c> if the value did not exist.</returns>
    public object? Get(string configName)
    {
        return !IsInitialized ? default
            : Get(new ConfigOption(configName, null));
    }

    /// <summary>
    /// Set a configuration value specified by the configuration <paramref name="option"/>.
    /// </summary>
    /// <param name="option">The <see cref="ConfigOption"/> containing information about the value to set.</param>
    /// <returns><c>true</c> if the value was set successfully; <c>false</c> otherwise.</returns>
    public bool Set(ConfigOption option)
    {
        return IsInitialized && Set(option, CurrentConfig);
    }

    /// <summary>
    /// Set a configuration <paramref name="value"/> specified by <paramref name="configName"/>.
    /// </summary>
    /// <param name="configName">The name of the configuration to set.</param>
    /// <param name="value">The value to set.</param>
    /// <returns><c>true</c> if the value was set successfully; <c>false</c> otherwise.</returns>
    public bool Set(string configName, object? value)
    {
        return IsInitialized && Set(new ConfigOption(configName, value), CurrentConfig);
    }

    /// <summary>
    /// Set a configuration option specified by <paramref name="option"/>.
    /// </summary>
    /// <param name="option">The configuration option to set.</param>
    /// <param name="instance">The instance of the configuration to set the option on.</param>
    /// <returns><c>true</c> if the value was set successfully; <c>false</c> otherwise.</returns>
    private bool Set(ConfigOption option, object? instance)
    {
        if (!IsInitialized) return false;
        if (instance == null) return false;

        foreach (var result in Reflect.GetAttributesRecurse<ConfigAttribute>(instance, instance.GetType()))
        {
            if (!((ConfigAttribute)result.Attribute).Name.Equals(option.Name)) continue;
            if (!option.ArrayIndexExists)
            {
                result.Property.SetValue(result.Instance, option.Value);
                return true;
            }

            var currentVal = result.Property.GetValue(result.Instance);
            if (currentVal == null || !Reflect.IsList(currentVal.GetType())) continue;
            
            var elementType = Reflect.GetCollectionElementType(currentVal.GetType());
            if (elementType == null) continue;
            
            var newList = Activator.CreateInstance(currentVal.GetType()) as IList;
                
            if (currentVal is not IList currentList) continue;
            if (newList == null) continue;
                
            foreach (var t in currentList)
                newList.Add(t);

            newList[option.ArrayIndexer] = option.Value;
            result.Property.SetValue(result.Instance, newList);

            return true;
        }
        return false;
    }
}