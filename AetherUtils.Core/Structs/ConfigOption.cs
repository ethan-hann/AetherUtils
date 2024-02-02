namespace AetherUtils.Core.Structs
{
    /// <summary>
    /// Represents a single configuration option and it's value. This option can contain a single-dimensional list indexer:
    /// <c>optionName[0]</c> - get/set the configuration with name <c>optionName</c> at index 0, where <c>optionName</c> is a list.
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

        public ConfigOption(string option, object? value)
        {
            if (!option.Contains('['))
                Name = option;
            else //If the name contains '[]', parse out the number between brackets and use for array index of list config value.
            {
                string optionName = option[..option.IndexOf('[')];
                string extraIndex = option[(option.IndexOf('[') + 1)..];
                extraIndex = extraIndex.Remove(extraIndex.IndexOf(']'));

                Name = optionName;

                if (int.TryParse(extraIndex, out int result))
                {
                    ArrayIndexer = result;
                }
            }

            Value = value;
        }
    }
}
