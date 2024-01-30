using AetherUtils.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Configuration
{
    public class TestNest
    {
        [Config("testProp")]
        public string TestProp { get; set; } = "TestNest String Property";

        [Config("testPropList")]
        public List<string> TestPropList { get; set; } = new List<string>() { "Item 1", "Item 2", "Item 3" };

        [Config("testNest2")]
        public TestNest2 TestNest2 { get; set; } = new TestNest2();
    }
}
