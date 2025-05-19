// LogOptions.cs : AetherUtils
// Copyright (C) 2025  Ethan Hann
// 
// MIT License
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using AetherUtils.Core.Attributes;
using JetBrains.Annotations;

namespace AetherUtils.Core.Configuration;

/// <summary>
///     A <a href="https://en.wikipedia.org/wiki/Data_transfer_object">DTO</a> representing options for how log
///     files are handled and formatted for an application.
/// </summary>
public sealed class LogOptions
{
    /// <summary>
    ///     The name of the application doing the logging.
    /// </summary>
    [Config("appName", "The name of the application doing the logging.", "MyApp")]
    public string AppName { get; [UsedImplicitly] set; } = string.Empty;

    /// <summary>
    ///     The directory that the log file should be saved to.
    /// </summary>
    [Config("logFileDirectory", "The directory that the log file should be saved to.", @"%TEMP%\logs")]
    public string LogFileDirectory { get; [UsedImplicitly] set; } = string.Empty;

    /// <summary>
    ///     Indicates if the log file name should include the current formatted <see cref="DateTime" /> the file was created.
    /// </summary>
    [Config("includeDateTime", "Indicates if the log file name should include the current formatted datetime the file was created.", false)]
    public bool IncludeDateTime { get; [UsedImplicitly] set; }

    /// <summary>
    ///     Indicates if the log file name should include the date only, instead of the full formatted <see cref="DateTime" />.
    /// </summary>
    [Config("includeDateOnly", "Indicates if the log file name should include the date only, instead of the full formatted datetime.", true)]
    public bool IncludeDateOnly { get; [UsedImplicitly] set; }

    /// <summary>
    ///     Indicates whether a new log file should be created for every new launch of the application.
    /// </summary>
    [Config("newFileEveryLaunch", "Indicates whether a new log file should be created for every new launch of the application.", false)]
    public bool NewFileEveryLaunch { get; [UsedImplicitly] set; }

    /// <summary>
    ///     Indicates whether the log should write to the system console in addition to a log file.
    /// </summary>
    [Config("writeLogToConsole", "Indicates whether the log should write to the system console in addition to a log file.", true)]
    public bool WriteLogToConsole { get; [UsedImplicitly] set; }

    /// <summary>
    ///     Specifies the default log layout to use for NLog.
    /// </summary>
    [Config("logLayout", "Specifies the default log layout to use for NLog.", "${longdate}|${level:uppercase=true}|${logger}|${message:withexception=true}")]
    public string LogLayout { get; [UsedImplicitly] set; } = "${longdate}|${level:uppercase=true}|${logger}|${message:withexception=true}";

    /// <summary>
    ///     Specifies the header to add at the top of each log file.
    /// </summary>
    [Config("logHeader", "Specifies the header to add at the top of each log file.", "")]
    public string LogHeader { get; [UsedImplicitly] set; } = string.Empty;

    /// <summary>
    ///     Specifies the footer to add at the end of each log file.
    /// </summary>
    [Config("logFooter", "Specifies the footer to add at the end of each log file.", "")]
    public string LogFooter { get; [UsedImplicitly] set; } = string.Empty;
}