using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Structs
{
    /// <summary>
    /// Represents a single configuration option. This option can contain a single-dimensional array indexer:
    /// <c>optionName[0]</c> - get/set the configuration with name <c>optionName</c> at index 0.
    /// </summary>
    public struct ConfigOption
    {
        public string Name { get; set; }
        public string ExtraOptions { get; set; }
        public object? Value { get; set; }
        public bool IsExtraOptionsArrayIndex { get; set; } = false;

        public ConfigOption(string name, object? value)
        {
            if (name.Contains('['))
            {
                string option = name[..name.IndexOf('[')];
                string extraOption = name.Substring(name.IndexOf('[') + 1, name.Length - name.IndexOf(']'));
                Name = option;
                ExtraOptions = extraOption;
                IsExtraOptionsArrayIndex = true;
            }
            else
            {
                Name = name;
                ExtraOptions = string.Empty;
                IsExtraOptionsArrayIndex = false;
            }

            Value = value;
        }
    }
}
