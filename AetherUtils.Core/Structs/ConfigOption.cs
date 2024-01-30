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
        public int ArrayIndexer { get; set; } = -1;
        public object? Value { get; set; }
        public readonly bool ArrayIndexExists
        {
            get { return ArrayIndexer > -1; }
        }

        public ConfigOption(string name, object? value)
        {
            if (!name.Contains('['))
                Name = name;
            else //If the name contains '[]', parse out the number between brackets and use for array index of list config value.
            {
                string option = name[..name.IndexOf('[')];
                string extraOption = name[(name.IndexOf('[') + 1)..];
                extraOption = extraOption.Remove(extraOption.IndexOf(']'));
                Name = option;
                if (int.TryParse(extraOption, out int result))
                {
                    ArrayIndexer = result;
                }
            }

            Value = value;
        }
    }
}
