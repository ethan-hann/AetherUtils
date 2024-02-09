using AetherUtils.Core.Attributes;

namespace AetherUtils.Core.Configuration;

/// <summary>
/// <a href="https://en.wikipedia.org/wiki/Data_transfer_object">DTO</a> representing the default, bare configuration
/// of a new application. This class can be used as is,
/// or a new class can be created to store the configuration for an application.<br/>
/// <para>If a new class is needed, it must be a DTO and it's properties
/// should be marked with <see cref="ConfigAttribute"/> attributes in order to be saved and loaded from
/// disk by a <see cref="ConfigManager{T}"/>.</para>
/// <remarks>
/// This class can contain instances of other DTO classes so long as those classes also have
/// the <see cref="ConfigAttribute"/> on their properties.
/// </remarks>
/// </summary>
public sealed class DefaultConfig
{
    /// <summary>
    /// The connection string used for connecting to a database.
    /// </summary>
    [Config("connectionString")]
    public string ConnectionString { get; set; } = "host=;port=;user=;password=;database=;";

    /// <summary>
    /// The full path to a valid license file for an application.
    /// </summary>
    [Config("licenseFile")]
    public string LicenseFile { get; set; } = string.Empty;

    /// <summary>
    /// A collection of options used for logging.
    /// </summary>
    [Config("logOptions")]
    public LogOptions LogOptions { get; set; } = new LogOptions();
}