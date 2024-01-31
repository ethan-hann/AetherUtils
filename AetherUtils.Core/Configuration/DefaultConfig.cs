using AetherUtils.Core.Attributes;

namespace AetherUtils.Core.Configuration;

/// <summary>
/// Class representing the default, bare configuration of a new application. This class can be used as is,
/// or a new class can be created to store the configuration. If a new class is created, it must be a DTO class and it's properties
/// should be marked with <see cref="ConfigAttribute"/> attributes in order to be saved and loaded from disk by the <see cref="ConfigManager{T}"/>.
/// </summary>
public class DefaultConfig
{
    /// <summary>
    /// The connection string used for connecting to a MariaDB database.
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