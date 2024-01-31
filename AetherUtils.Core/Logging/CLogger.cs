using AetherUtils.Core.Configuration;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Logging
{
    /// <summary>
    /// Provides static methods to facilitate logging to a file as well as to the console for an application.
    /// </summary>
    public static class CLogger
    {
        private static bool _isInitialized = false;

        /// <summary>
        /// Initialize the logger for the application. This should be called before any logging takes place; 
        /// usually, directly after reading or creating the initial configuration for the application.
        /// </summary>
        /// <param name="options">The <see cref="LogOptions"/> that are used to setup the logger.</param>
        public static void Initialize(LogOptions options)
        {
            var config = new LoggingConfiguration();

            //Targets for logging
            var logFile = new FileTarget("logFile")
            {
                Header = options.LogHeader,
                Footer = options.LogFooter,
                Layout = options.LogLayout
            };

            string fileFullPath = options.LogFileDirectory;
            if (!options.AppName.Equals(string.Empty))
                fileFullPath = Path.Combine(fileFullPath, options.AppName, ".log");
            else
                fileFullPath = Path.Combine(fileFullPath, "appLog.log");

            fileFullPath = Files.FileHelper.ExpandPath(fileFullPath);

            //Create log directory if it doesn't exist.
            Files.FileHelper.CreateDirectories(fileFullPath, false);

            logFile.FileName = fileFullPath;

            //FileName = Path.Combine(options.LogFileDirectory, options.AppName, ".log"),

            var logConsole = new ConsoleTarget("logConsole")
            {
                DetectConsoleAvailable = true,
                Header = options.LogHeader,
                Footer = options.LogFooter,
                Layout = options.LogLayout
            };

            if (options.WriteLogToConsole)
                config.AddRule(LogLevel.Info, LogLevel.Fatal, logConsole);

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logFile);

            LogManager.Configuration = config;
            _isInitialized = true;
        }

        /// <summary>
        /// Get a logger reference based on the type of class.
        /// </summary>
        /// <typeparam name="T">The class type to get the logger of.</typeparam>
        /// <returns>A <see cref="Logger"/> object that can be used for logging. Returns default name if logging has not been initialized via <see cref="Initialize(LogOptions)"/></returns>
        public static Logger GetCurrentLogger<T>() where T : class
        {
            if (!_isInitialized) { return LogManager.GetCurrentClassLogger(); }
            return LogManager.GetLogger($"{typeof(T).FullName}");
        }

        /// <summary>
        /// Get a logger reference based on the type of class as well as an additional name identifier.
        /// </summary>
        /// <typeparam name="T">The class type to get the logger of.</typeparam>
        /// <param name="name">The additional name to add to the logger. (i.e., the method name)</param>
        /// <returns>A <see cref="Logger"/> object that can be used for logging. Returns default name if logging has not been initialized.</returns>
        public static Logger GetCurrentLogger<T>(string name) where T : class
        {
            if (!_isInitialized) { return LogManager.GetCurrentClassLogger(); }
            return LogManager.GetLogger($"{typeof(T).FullName}.{name}");
        }
    }
}
