using AetherUtils.Core.Attributes;

namespace AetherUtils.Core.Configuration;

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