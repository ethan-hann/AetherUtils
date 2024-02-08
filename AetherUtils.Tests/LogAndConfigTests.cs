using AetherUtils.Core.Configuration;
using AetherUtils.Core.Logging;
using AetherUtils.Core.Structs;
using NLog;

namespace AetherUtils.Tests
{
    public class LogAndConfigTests
    {
        YamlConfigManager configManager;

        [SetUp]
        public void Setup()
        {
            configManager = new YamlConfigManager("config\\test.yaml");
            if (!configManager.ConfigExists)
                configManager.CreateConfig();
            Assert.That(configManager.Load(), Is.True);
        }

        [Test]
        public void TestLoggerCreate()
        {
            LogOptions? options = (LogOptions?)configManager.Get("logOptions");
            Assert.That(options, Is.Not.Null);

            CLogger.Initialize(options);
            Assert.That(CLogger.IsInitialized, Is.True);

            Logger log = CLogger.GetCurrentLogger<YamlConfigManager>("TestLoggerCreate()");
            log.Debug("Test Log Message");
        }

        [Test]
        public void TestChangeConfigValue()
        {
            if (!configManager.ConfigExists)
                configManager.CreateConfig();
            Assert.That(configManager.Load(), Is.True);

            string testString = "Test String";
            ConfigOption option = new("connectionString", testString);
            Assert.Multiple(() =>
            {
                Assert.That(configManager.Set(option), Is.True);
                Assert.That(configManager.Save(), Is.True);
                Assert.That(configManager.Load(), Is.True);
            });

            var readString = configManager.Get(option);
            readString = readString as string;
            Assert.That(readString, Is.Not.Null);
            Assert.That(readString, Is.EqualTo(testString));
        }
    }
}