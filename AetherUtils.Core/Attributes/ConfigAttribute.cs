namespace AetherUtils.Core.Attributes;

/// <summary>
/// Specifies the name of a property in a YAML configuration file.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ConfigAttribute(string name) : Attribute
{
    /// <summary>
    /// The YAML name of the property in the configuration file.
    /// </summary>
    public string Name { get; set; } = name;

    public ConfigAttribute() : this(string.Empty) { }
}