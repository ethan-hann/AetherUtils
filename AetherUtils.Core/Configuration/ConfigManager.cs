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
    private IDeserializer _deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IncludeNonPublicProperties().Build();
    
    private ISerializer _serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IncludeNonPublicProperties().Build();

    protected T? CurrentConfig { get; set; }

    public string ConfigFilePath { get; set; } = configFilePath;

    public bool IsInitialized { get => CurrentConfig != null; }

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
    /// Get a configuration value from the current configuration.
    /// </summary>
    /// <param name="configName">The name of the configuration, determined by its <see cref="ConfigAttribute"/>.</param>
    /// <returns>The configuration value, or <c>null</c> if no value was found.</returns>
    public object? Get(string configName)
    {
        if (!IsInitialized) return default;
        return Get(configName, CurrentConfig);
    }

    private object? Get(string configName, object? instance)
    {
        if (!IsInitialized) return default;
        if (instance == null) return default;

        var type = instance.GetType();

        foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            IEnumerable<ConfigAttribute> configAttributes = property.GetCustomAttributes<ConfigAttribute>();
            ConfigAttribute? attrib = configAttributes.FirstOrDefault(e => e.Name.Equals(configName));
            if (attrib == null)
            {
                if (property.PropertyType.FullName != "System.String" && !property.PropertyType.IsPrimitive)
                    return Get(configName, property.GetValue(instance));
                else continue;
            }

            return property.GetValue(instance);
        }
        return null; //The property with the specified config name was not found.
    }

    /// <summary>
    /// Set a configuration value specified by <paramref name="option"/>.
    /// </summary>
    /// <param name="option">The <see cref="ConfigOption"/> containing information about the value to set.</param>
    public void Set(ConfigOption option)
    {
        if (!IsInitialized) return;
        Set(option, CurrentConfig);
    }

    /// <summary>
    /// Set a configuration value <paramref name="value"/> specified by <paramref name="configName"/>.
    /// </summary>
    /// <param name="configName">The name of the configuration to set.</param>
    /// <param name="value">The value to set.</param>
    public void Set(string configName, object? value)
    {
        if (!IsInitialized) return;
        Set(new ConfigOption(configName, value), CurrentConfig);
    }

    private bool Set(ConfigOption option, object? instance)
    {
        if (!IsInitialized) return false;
        if (instance == null) return false;

        var type = instance.GetType();

        foreach (var result in Reflect.GetAttributeList<ConfigAttribute>(instance, instance.GetType()))
        {
            var attrib = result.Property.GetCustomAttribute<ConfigAttribute>();
            if (attrib != null) 
            {
                if (attrib.Name.Equals(option.Name))
                {
                    if (option.IsExtraOptionsArrayIndex)
                    {
                        if (int.TryParse(option.ExtraOptions, out int index))
                        {
                            var currentVal = result.Property.GetValue(result.Instance);
                            if (Reflect.IsList(currentVal.GetType()))
                            {
                                Type? elementType = Reflect.GetCollectionElementType(currentVal.GetType());
                                if (elementType != null)
                                {
                                    IList? newList = Activator.CreateInstance(currentVal.GetType()) as IList;
                                    IList? currentList = currentVal as IList;
                                    for (int i = 0; i < currentList.Count; i++)
                                        newList.Add(currentList[i]);

                                    newList[index] = option.Value;
                                    result.Property.SetValue(result.Instance, newList);
                                }
                            }
                        }
                    }
                    else
                    {
                        result.Property.SetValue(result.Instance, option.Value);
                    }
                    //var prop = (from property in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                    //            where property.PropertyType.Equals(result.Property) select property).FirstOrDefault();
                    //PropertyInfo pInfo = result.Property ?? throw new Exception($"Property {option.Name.ToLower()} not found");

                    //var currentVal = pInfo.GetValue(result.Instance, null);
                    
                    //result.Property.SetValue(instance, option.Value);
                }
            }
        }
        return true;
    }
}