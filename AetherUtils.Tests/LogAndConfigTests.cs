// LogAndConfigTests.cs : AetherUtils
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
using AetherUtils.Core.Logging;
using AetherUtils.Core.Structs;

namespace AetherUtils.Tests;

public class LogAndConfigTests
{
    private const string configFilePath = "config\\config.yaml";
    private YamlConfigManager? _configManager;

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