using AetherUtils.Core.Attributes;
using AetherUtils.Core.Configuration;
using AetherUtils.Core.Logging;
using AetherUtils.Core.Structs;

namespace AetherUtils.Tests;

public class LogAndConfigTests
{
    private const string configFilePath = "config\\config.yaml";
    private ConfigManager<DefaultConfig>? _configManager;

    [SetUp]
    public void Setup()
    {
        _configManager = ConfigManagerFactory.CreateAndLoad<DefaultConfig>(configFilePath);
    }

    [Test]
    public void TestConfigCreate() => Assert.That(_configManager is { IsInitialized: true }, Is.True);

    [Test]
    public void TestLoggerCreate()
    {
        var options = (LogOptions?)_configManager?.Get("logOptions");
        Assert.That(options, Is.Not.Null);
        AuLogger.Initialize(options);
        Assert.That(AuLogger.IsInitialized, Is.True);
        AuLogger.GetCurrentLogger<ConfigManager<DefaultConfig>>("TestLoggerCreate()").Debug("Test Log Message");
    }

    [Test]
    public void TestChangeConfigValue()
    {
        const string testString = "Test String";
        ConfigOption option = new("connectionString", testString);
        Assert.Multiple(() =>
        {
            Assert.That(_configManager?.Set(option), Is.True);
            Assert.That(_configManager?.Save(), Is.True);
            Assert.That(_configManager?.Load(), Is.True);
        });
        var readString = _configManager?.Get(option) as string;
        Assert.That(readString, Is.EqualTo(testString));
    }

    [Test]
    public void TestApplyDefaultValues()
    {
        var config = _configManager?.GetConfig();
        Assert.That(config, Is.Not.Null);
        config!.ConnectionString = null!;
        ConfigDefaultsApplier.ApplyDefaults(config);
        Assert.That(config.ConnectionString, Is.EqualTo("host=;port=;user=;password=;database=;"));
    }

    [Test]
    public void TestDefaultAppliedAfterYamlLoad()
    {
        var path = "config\\test-default.yaml";
        File.WriteAllText(path, "licenseFile: 'test.lic'");
        var manager = ConfigManagerFactory.CreateAndLoad<DefaultConfig>(path);
        Assert.That(manager?.GetConfig()?.ConnectionString, Is.EqualTo("host=;port=;user=;password=;database=;"));
    }

    [Test]
    public async Task TestAsyncLoadWithDefaults()
    {
        var path = "config\\test-default-async.yaml";
        File.WriteAllText(path, "licenseFile: 'test.lic'");
        var manager = await ConfigManagerFactory.CreateAndLoadAsync<DefaultConfig>(path);
        Assert.That(manager?.GetConfig()?.ConnectionString, Is.EqualTo("host=;port=;user=;password=;database=;"));
    }

    [Test]
    public async Task TestAsyncLoadWithApplyAndSave()
    {
        var path = "config\\test-default-async-save.yaml";
        File.WriteAllText(path, "licenseFile: 'test.lic'");
        await ConfigManagerFactory.CreateAndLoadAsync<DefaultConfig>(path, true);
        var reloaded = new ConfigManager<DefaultConfig>(path);
        reloaded.Load();
        Assert.That(reloaded.GetConfig()?.ConnectionString, Is.EqualTo("host=;port=;user=;password=;database=;"));
    }

    private sealed class DefaultConfigWithInvalidDefault
    {
        [Config("port", "Port", "not-an-int")]
        public int Port { get; set; }
    }

    [Test]
    public void TestInvalidDefaultConversion()
    {
        var config = new DefaultConfigWithInvalidDefault();
        Assert.DoesNotThrow(() => ConfigDefaultsApplier.ApplyDefaults(config));
        Assert.That(config.Port, Is.EqualTo(0));
    }

    private sealed class ReadOnlyConfig
    {
        [Config("readonlyVal", "Should not apply", 999)]
        public int ReadOnlyVal => 123;
    }

    [Test]
    public void TestReadOnlyPropertyIgnored()
    {
        var config = new ReadOnlyConfig();
        Assert.DoesNotThrow(() => ConfigDefaultsApplier.ApplyDefaults(config));
        Assert.That(config.ReadOnlyVal, Is.EqualTo(123));
    }

    private sealed class ListConfig
    {
        [Config("servers", "Default servers", new[] { "localhost" })]
        public List<string> Servers { get; set; } = null!;
    }

    [Test]
    public void TestDefaultListApplied()
    {
        var config = new ListConfig { Servers = null! };
        ConfigDefaultsApplier.ApplyDefaults(config);
        Assert.That(config.Servers, Does.Contain("localhost"));
    }

    private struct Resolution { public int Width, Height; }
    private sealed class StructConfig
    {
        [Config("resolution", "Screen size", null)]
        public Resolution Resolution { get; set; }
    }

    [Test]
    public void TestStructDefaultSkippedForNull()
    {
        var config = new StructConfig();
        ConfigDefaultsApplier.ApplyDefaults(config);
        Assert.That(config.Resolution.Width, Is.EqualTo(0));
    }

    private sealed class DictionaryConfigTestModel
    {
        [Config("paths", "Path mappings", new[] { "temp=%TEMP%", "logs=C:\\Logs" })]
        public Dictionary<string, string> Paths { get; set; } = null!;
    }

    [Test]
    public void TestDefaultDictionaryApplied()
    {
        var config = new DictionaryConfigTestModel { Paths = null! };
        ConfigDefaultsApplier.ApplyDefaults(config);
        Assert.That(config.Paths, Is.Not.Null);
        Assert.That(config.Paths, Contains.Key("temp"));
        Assert.That(config.Paths["temp"], Is.Not.Null);
    }

    private sealed class NullableConfigTestModel
    {
        [Config("retryCount", "Retries", 3)]
        public int? RetryCount { get; set; }

        [Config("enabled", "Enable it", true)]
        public bool? Enabled { get; set; }
    }

    [Test]
    public void TestNullableDefaultsApplied()
    {
        var config = new NullableConfigTestModel();
        ConfigDefaultsApplier.ApplyDefaults(config);
        Assert.That(config.RetryCount, Is.EqualTo(3));
        Assert.That(config.Enabled, Is.True);
    }
}