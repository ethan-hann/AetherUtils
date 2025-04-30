// AuLogger.cs : AetherUtils
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

using AetherUtils.Core.Configuration;
using AetherUtils.Core.Files;
using JetBrains.Annotations;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace AetherUtils.Core.Logging;

/// <summary>
///     Provides static methods to facilitate logging to a file as well as to the console for an application.<br />
///     This class is a wrapper around <a href="https://nlog-project.org/">NLog</a>.
/// </summary>
public static class AuLogger
{
    /// <summary>
    ///     Get a value indicating if the logger has been initialized.
    /// </summary>
    public static bool IsInitialized { get; private set; }

    /// <summary>
    ///     Initialize the logger for the application. This should be called before any logging takes place;
    ///     usually, directly after reading or creating the initial configuration for the application.
    /// </summary>
    /// <param name="options">The <see cref="LogOptions" /> that are used to set up the logger.</param>
    public static void Initialize(LogOptions options)
    {
        var config = new LoggingConfiguration();

        //Set up targets for logging
        var logFile = CreateLogFileFromOptions(options);

        if (options.WriteLogToConsole)
        {
            var logConsole = CreateLogConsoleFromOptions(options);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logConsole);
        }

        config.AddRule(LogLevel.Debug, LogLevel.Fatal, logFile);

        LogManager.Configuration = config;
        IsInitialized = true;
    }

    private static FileTarget CreateLogFileFromOptions(LogOptions options)
    {
        var logFile = new FileTarget("logFile")
        {
            ArchiveOldFileOnStartup = options.NewFileEveryLaunch,
            FileNameKind = FilePathKind.Absolute,
            Layout = options.LogLayout
        };

        var fileName = (options.AppName.Equals(string.Empty) ? "appLog" : options.AppName)
                       + (options.IncludeDateOnly ? DateTime.Now.ToString("_MM-dd-yyyy")
                           : options.IncludeDateTime ? DateTime.Now.ToString("_MM-dd-yyyy-hh-mm-ss-ff") : string.Empty)
                       + ".log";

        var fullPath = Path.Combine(options.LogFileDirectory, fileName);
        fullPath = FileHelper.ExpandPath(fullPath);

        //Create log directory if it doesn't exist.
        FileHelper.CreateDirectories(fullPath, false);
        logFile.FileName = fullPath;

        if (!options.LogHeader.Equals(string.Empty))
            logFile.Header = options.LogHeader;
        if (!options.LogFooter.Equals(string.Empty))
            logFile.Footer = options.LogFooter;

        //Set up appropriate locking and access
        logFile.KeepFileOpen = false;
        logFile.ConcurrentWrites = true;

        return logFile;
    }

    private static ConsoleTarget CreateLogConsoleFromOptions(LogOptions options)
    {
        var logConsole = new ConsoleTarget("logConsole")
        {
            DetectConsoleAvailable = true,
            Layout = options.LogLayout
        };

        if (!options.LogHeader.Equals(string.Empty))
            logConsole.Header = options.LogHeader;
        if (!options.LogFooter.Equals(string.Empty))
            logConsole.Footer = options.LogFooter;

        return logConsole;
    }

    /// <summary>
    ///     Get a logger reference based on the type of class.
    /// </summary>
    /// <typeparam name="T">The class type to get the logger of.</typeparam>
    /// <returns>
    ///     A <see cref="Logger" /> object that can be used for logging. Returns default name if logging has not been
    ///     initialized via <see cref="Initialize(LogOptions)" />
    /// </returns>
    [UsedImplicitly]
    public static Logger GetCurrentLogger<T>() where T : class =>
        !IsInitialized ? LogManager.GetCurrentClassLogger()
            : LogManager.GetLogger($"{typeof(T).FullName}");

    /// <summary>
    ///     Get a logger reference based on the type of class as well as an additional name identifier.
    /// </summary>
    /// <typeparam name="T">The class type to get the logger of.</typeparam>
    /// <param name="name">The additional name to add to the logger. (i.e., the method name)</param>
    /// <returns>
    ///     A <see cref="Logger" /> object that can be used for logging. Returns default name if logging has not been
    ///     initialized.
    /// </returns>
    public static Logger GetCurrentLogger<T>(string name) where T : class =>
        !IsInitialized ? LogManager.GetCurrentClassLogger()
            : LogManager.GetLogger($"{typeof(T).FullName}.{name}");

    /// <summary>
    ///     Get a logger reference based on the specified name.
    /// </summary>
    /// <param name="name">The name of the logger to get.</param>
    /// <returns>
    ///     A <see cref="Logger" /> object that can be used for logging. Returns default name if logging has not been
    ///     initialized.
    /// </returns>
    [UsedImplicitly]
    public static Logger GetCurrentLogger(string name) =>
        !IsInitialized ? LogManager.GetCurrentClassLogger()
            : LogManager.GetLogger(name);
}