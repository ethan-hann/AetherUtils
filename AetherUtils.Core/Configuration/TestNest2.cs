using AetherUtils.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Configuration
{
    public class TestNest2
    {
        [Config("testPropListInts")]
        public List<int> TestPropListInts { get; set; } = new List<int>()
        {
            1, 6, 19, 20, 212
        };
    }
}
