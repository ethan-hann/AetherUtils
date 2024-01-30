using AetherUtils.Core.Attributes;

namespace AetherUtils.Core.Configuration;

/// <summary>
/// Represents options for how log files are handled and formatted for an application.
/// </summary>
public class LogOptions
{
    /// <summary>
    /// The directory that the log file should be saved to.
    /// </summary>
    [Config("logFileDirectory")]
    public string LogFileDirectory { get; set; } = "%TEMP%\\logs";

    /// <summary>
    /// Indicates if the log file name should include the current DateTime the file was created.
    /// </summary>
    [Config("includeDateTime")]
    public bool IncludeDateTime { get; set; } = true;

    /// <summary>
    /// Indicates if the log file name should include the date only, instead of the full DateTime.
    /// </summary>
    [Config("includeDateOnly")]
    public bool IncludeDateOnly { get; set; } = false;

    /// <summary>
    /// Indicates whether a new log file should be created every new launch of the application.
    /// </summary>
    [Config("newFileEveryLaunch")]
    public bool NewFileEveryLaunch { get; set; } = true;

    [Config("testNest")]
    public TestNest TestNest { get; set; } = new TestNest();
}