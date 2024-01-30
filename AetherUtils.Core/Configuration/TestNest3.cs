using AetherUtils.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Configuration
{
    public class TestNest3
    {
        [Config("testPropListDoubles")]
        public List<double> TestPropListDoubles { get; set; } = new List<double>()
        {
            0.5, 5.9, 52.21, 79.52
        };
    }
}
