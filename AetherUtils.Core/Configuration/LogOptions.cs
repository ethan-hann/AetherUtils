using AetherUtils.Core.Attributes;

namespace AetherUtils.Core.Configuration;

/// <summary>
/// A <a href="https://en.wikipedia.org/wiki/Data_transfer_object">DTO</a> representing options for how log
/// files are handled and formatted for an application.
/// </summary>
public sealed class LogOptions
{
    /// <summary>
    /// The name of the application doing the logging.
    /// </summary>
    [Config("appName")]
    public string AppName { get; set; } = string.Empty;

    /// <summary>
    /// The directory that the log file should be saved to.
    /// </summary>
    [Config("logFileDirectory")]
    public string LogFileDirectory { get; set; } = @"%TEMP%\logs";

    /// <summary>
    /// Indicates if the log file name should include the current formatted <see cref="DateTime"/> the file was created.
    /// </summary>
    [Config("includeDateTime")]
    public bool IncludeDateTime { get; set; } = false;

    /// <summary>
    /// Indicates if the log file name should include the date only, instead of the full formatted <see cref="DateTime"/>.
    /// </summary>
    [Config("includeDateOnly")]
    public bool IncludeDateOnly { get; set; } = true;

    /// <summary>
    /// Indicates whether a new log file should be created for every new launch of the application.
    /// </summary>
    [Config("newFileEveryLaunch")]
    public bool NewFileEveryLaunch { get; set; } = false;

    /// <summary>
    /// Indicates whether the log should write to the system console in addition to a log file.
    /// </summary>
    [Config("writeLogToConsole")]
    public bool WriteLogToConsole { get; set; } = true;

    /// <summary>
    /// Specifies the default log layout to use for NLog.
    /// </summary>
    public string LogLayout { get; set; } = "${longdate}|${level:uppercase=true}|${logger}|${message:withexception=true}";

    /// <summary>
    /// Specifies the header to add at the top of each log file.
    /// </summary>
    [Config("logHeader")]
    public string LogHeader { get; set; } = string.Empty;

    /// <summary>
    /// Specifies the footer to add at the end of each log file.
    /// </summary>
    [Config("logFooter")]
    public string LogFooter { get; set; } = string.Empty;
}