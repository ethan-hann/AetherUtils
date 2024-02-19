using AetherUtils.Core.Configuration;
using AetherUtils.Core.Files;

namespace AetherUtils.Tests
{
    public class JsonTests
    {
        private readonly string configPath = "json\\config.json";

        [Test]
        public void TestLoadingAndSavingJson()
        {
            var config = new DefaultConfig();
            var jsonHelper = new Json<DefaultConfig>();
            
            jsonHelper.SaveJson(configPath, config);
            Assert.That(FileHelper.DoesFileExist(configPath), Is.True);
            
            var config2 = jsonHelper.LoadJson(configPath);
            Assert.That(config2, Is.Not.Null);
        }
    }
}
