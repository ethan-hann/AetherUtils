using AetherUtils.Core.Configuration;
using AetherUtils.Core.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Tests
{
    public class JsonTests
    {
        DefaultConfig config;
        Json<DefaultConfig> jsonHelper;

        [SetUp] 
        public void SetUp()
        {
            config = new DefaultConfig();
            jsonHelper = new Json<DefaultConfig>();
        }

        [Test]
        public void TestSavingJson()
        {
            jsonHelper.SaveJSON("json\\config.json", config);
            Assert.That(Path.Exists("json\\config.json"), Is.True);
        }

        [Test]
        public void TestLoadingJson()
        {
            config = jsonHelper.LoadJSON("json\\config.json");
            Assert.That(config, Is.Not.Null);
        }
    }
}
