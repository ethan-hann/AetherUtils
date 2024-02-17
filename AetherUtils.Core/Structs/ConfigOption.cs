namespace AetherUtils.Core.Structs
{
    /// <summary>
    /// Represents a single configuration option and it's value. This option can contain a single-dimensional list indexer.
    /// </summary>
    /// <remarks>
    /// <c> var option = new ConfigOption("listOption[0]", 2);</c> <br/>
    /// would allow you to change the value of the listOption at index 0 to the value 2.
    /// </remarks>
    public readonly struct ConfigOption
    {
        public string Name { get; }
        public int ArrayIndexer { get; } = -1;
        public object? Value { get; }
        public bool ArrayIndexExists => ArrayIndexer > -1;

        public ConfigOption(string option, object? value)
        {
            if (!option.Contains('['))
                Name = option;
            else //If the name contains '[]', parse out the number between brackets and use for array index of list config value.
            {
                var optionName = option[..option.IndexOf('[')];
                var extraIndex = option[(option.IndexOf('[') + 1)..];
                extraIndex = extraIndex.Remove(extraIndex.IndexOf(']'));

                Name = optionName;

                if (int.TryParse(extraIndex, out var result))
                    ArrayIndexer = result;
            }

            Value = value;
        }
    }
}
