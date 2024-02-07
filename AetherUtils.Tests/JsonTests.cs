using AetherUtils.Core.Configuration;
using AetherUtils.Core.Files;

namespace AetherUtils.Tests
{
    public class JsonTests
    {
        DefaultConfig? config;
        Json<DefaultConfig> jsonHelper;
        private readonly string configPath = @"%temp%\AetherTests\json\config.json";

        [SetUp]
        public void SetUp()
        {
            config = new DefaultConfig();
            jsonHelper = new Json<DefaultConfig>();
        }

        [Test]
        public void TestSavingJson()
        {
            jsonHelper.SaveJson(configPath, config);
            Assert.That(FileHelper.DoesFileExist(configPath), Is.True);
        }

        [Test]
        public void TestLoadingJson()
        {
            config = jsonHelper.LoadJson(configPath);
            Assert.That(config, Is.Not.Null);
        }
    }
}
