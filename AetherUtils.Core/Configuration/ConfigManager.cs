using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using AetherUtils.Core.Attributes;
using AetherUtils.Core.Files;
using AetherUtils.Core.Reflection;
using AetherUtils.Core.Structs;
using Microsoft.Extensions.DependencyInjection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AetherUtils.Core.Configuration;

/// <summary>
/// This class cannot be instantiated. A child class must be created inheriting from <see cref="ConfigManager{T}"/>.
/// Handles saving, loading, and querying a generic configuration based on class <see cref="T"/>.
/// <para>
/// The custom class should have its properties related to configuration marked with a <see cref="ConfigAttribute"/>.
/// These properties are the only ones which will be serialized and de-serialized from disk.
/// </para>
/// </summary>
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

    protected T? CurrentConfig { get; set; }

    public string ConfigFilePath { get; set; } = configFilePath;

    public bool IsInitialized { get => CurrentConfig != null; }

    public bool ConfigExists { get => FileHelper.DoesFileExist(ConfigFilePath); }

    /// <summary>
    /// Create the configuration and optionally, save the file to disk.
    /// </summary>
    public abstract void CreateConfig(bool saveToDisk = true);

    /// <summary>
    /// Loads a configuration file from disk based on the <see cref="ConfigFilePath"/>.
    /// </summary>
    /// <returns><c>true</c> if the config loaded successfully; <c>false</c> otherwise.</returns>
    public virtual bool Load()
    {
        try
        {
            ConfigFilePath = FileHelper.ExpandPath(ConfigFilePath);
            var text = File.ReadAllText(ConfigFilePath);
            CurrentConfig = _deserializer.Deserialize<T>(text);
        } catch (Exception ex) { Debug.WriteLine(ex.StackTrace); return false; }
        
        return IsInitialized;
    }

    /// <summary>
    /// Saves a configuration file to disk based on the <see cref="CurrentConfig"/> and the <see cref="ConfigFilePath"/>.
    /// </summary>
    /// <returns><c>true</c> if the config saved successfully; <c>false</c> otherwise.</returns>
    public virtual bool Save()
    {
        try
        {
            ConfigFilePath = FileHelper.ExpandPath(ConfigFilePath);
            var serializedString = _serializer.Serialize(CurrentConfig);
            FileHelper.CreateDirectories(ConfigFilePath);
            File.WriteAllText(ConfigFilePath, serializedString);
            
            return FileHelper.DoesFileExist(ConfigFilePath, false);
        } catch (Exception ex) {Debug.WriteLine(ex.StackTrace); return false; }
    }

    /// <summary>
    /// Get a configuration value specified by <paramref name="option"/>.
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
            if (attrib != null)
            {
                if (attrib.Name.Equals(option.Name))
                {
                    if (!option.ArrayIndexExists)
                        return result.Property.GetValue(result.Instance);
                    else
                    {
                        var currentVal = result.Property.GetValue(result.Instance);
                        if (currentVal != null && Reflect.IsList(currentVal.GetType()))
                        {
                            try
                            {
                                IList? currentList = currentVal as IList;
                                return currentList[option.ArrayIndexer];
                            }
                            catch (Exception ex) { Debug.WriteLine(ex.Message); return false; }
                        }
                    }
                }
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
        if (!IsInitialized) return default;
        return Get(new ConfigOption(configName, null));
    }

    /// <summary>
    /// Set a configuration value specified by <paramref name="option"/>.
    /// </summary>
    /// <param name="option">The <see cref="ConfigOption"/> containing information about the value to set.</param>
    /// <returns><c>true</c> if the value was set successfully; <c>false</c> otherwise.</returns>
    public bool Set(ConfigOption option)
    {
        if (!IsInitialized) return false;
        return Set(option, CurrentConfig);
    }

    /// <summary>
    /// Set a configuration value <paramref name="value"/> specified by <paramref name="configName"/>.
    /// </summary>
    /// <param name="configName">The name of the configuration to set.</param>
    /// <param name="value">The value to set.</param>
    /// <returns><c>true</c> if the value was set successfully; <c>false</c> otherwise.</returns>
    public bool Set(string configName, object? value)
    {
        if (!IsInitialized) return false;
        return Set(new ConfigOption(configName, value), CurrentConfig);
    }

    private bool Set(ConfigOption option, object? instance)
    {
        if (!IsInitialized) return false;
        if (instance == null) return false;

        foreach (var result in Reflect.GetAttributesRecurse<ConfigAttribute>(instance, instance.GetType()))
        {
            if (((ConfigAttribute)result.Attribute).Name.Equals(option.Name))
            {
                if (!option.ArrayIndexExists)
                {
                    result.Property.SetValue(result.Instance, option.Value);
                    return true;
                }
                else
                {
                    var currentVal = result.Property.GetValue(result.Instance);
                    if (currentVal != null && Reflect.IsList(currentVal.GetType()))
                    {
                        Type? elementType = Reflect.GetCollectionElementType(currentVal.GetType());
                        if (elementType != null)
                        {
                            try
                            {
                                IList? newList = Activator.CreateInstance(currentVal.GetType()) as IList;
                                IList? currentList = currentVal as IList;
                                for (int i = 0; i < currentList.Count; i++)
                                    newList.Add(currentList[i]);

                                newList[option.ArrayIndexer] = option.Value;
                                result.Property.SetValue(result.Instance, newList);
                                return true;
                            }
                            catch (Exception ex) { Debug.WriteLine(ex.Message); return false; }
                        }
                    }
                }
            }
        }
        return false;
    }
}