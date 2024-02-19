using System.Diagnostics;
using AetherUtils.Core.Configuration;
using AetherUtils.Core.Logging;
using AetherUtils.Core.Structs;
using NLog;

namespace AetherUtils.Tests
{
    public class LogAndConfigTests
    {
        private YamlConfigManager? _configManager;
        private const string configFilePath = "config\\config.yaml";

        [SetUp]
        public void Setup()
        {
            _configManager = new YamlConfigManager(configFilePath);

            if (!_configManager.ConfigExists)
            {
                _configManager.CreateDefaultConfig();
                _configManager.Save();
            }
            else
                _configManager.Load();
        }
        
        [Test]
        public void TestConfigCreate()
        {
            Assert.That(_configManager is { IsInitialized: true }, Is.True);
        }

        [Test]
        public void TestLoggerCreate()
        {
            var options = (LogOptions?)_configManager?.Get("logOptions");
            Assert.That(options, Is.Not.Null);

            AuLogger.Initialize(options);
            Assert.That(AuLogger.IsInitialized, Is.True);

            var log = AuLogger.GetCurrentLogger<YamlConfigManager>("TestLoggerCreate()");
            log.Debug("Test Log Message");
        }

        [Test]
        public void TestChangeConfigValue()
        {
            Assert.That(_configManager is { IsInitialized: true }, Is.True);

            const string testString = "Test String";
            ConfigOption option = new("connectionString", testString);
            Assert.Multiple(() =>
            {
                Assert.That(_configManager != null && _configManager.Set(option), Is.True);
                Assert.That(_configManager != null && _configManager.Save(), Is.True);
                Assert.That(_configManager != null && _configManager.Load(), Is.True);
            });

            var readString = _configManager?.Get(option);
            readString = readString as string;
            Assert.That(readString, Is.Not.Null);
            Assert.That(readString, Is.EqualTo(testString));
        }
    }
}