using AetherUtils.Core.Configuration;
using AetherUtils.Core.Logging;
using AetherUtils.Core.Structs;
using NLog;

namespace AetherUtils.Tests
{
    public class LogAndConfigTests
    {
        private YamlConfigManager _configManager;

        [SetUp]
        public void Setup()
        {
            _configManager = new YamlConfigManager("config\\test.yaml");
            if (!_configManager.ConfigExists)
                _configManager.CreateConfig();
            Assert.That(_configManager.LoadAsync().Result, Is.True);
        }

        [Test]
        public void TestLoggerCreate()
        {
            LogOptions? options = (LogOptions?)_configManager.Get("logOptions");
            Assert.That(options, Is.Not.Null);

            CLogger.Initialize(options);
            Assert.That(CLogger.IsInitialized, Is.True);

            Logger log = CLogger.GetCurrentLogger<YamlConfigManager>("TestLoggerCreate()");
            log.Debug("Test Log Message");
        }

        [Test]
        public void TestChangeConfigValue()
        {
            if (!_configManager.ConfigExists)
                _configManager.CreateConfig();
            Assert.That(_configManager.LoadAsync().Result, Is.True);

            string testString = "Test String";
            ConfigOption option = new("connectionString", testString);
            Assert.Multiple(() =>
            {
                Assert.That(_configManager.Set(option), Is.True);
                Assert.That(_configManager.SaveAsync().Result, Is.True);
                Assert.That(_configManager.LoadAsync().Result, Is.True);
            });

            var readString = _configManager.Get(option);
            readString = readString as string;
            Assert.That(readString, Is.Not.Null);
            Assert.That(readString, Is.EqualTo(testString));
        }
    }
}