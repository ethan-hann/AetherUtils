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
        /// <summary>
        /// The name of the configuration option.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The array index for the option, if <see cref="Name"/> references a list.
        /// </summary>
        public int ArrayIndexer { get; } = -1;
        
        /// <summary>
        /// The value for the configuration option specified by <see cref="Name"/>.
        /// </summary>
        public object? Value { get; }
        
        /// <summary>
        /// Get a value indicating if this configuration option contains an array indexer.
        /// </summary>
        public bool ArrayIndexExists => ArrayIndexer > -1;

        /// <summary>
        /// Create a new configuration option with the specified 
        /// </summary>
        /// <param name="optionName">The name of the option.</param>
        /// <param name="value">The value for the option.</param>
        public ConfigOption(string optionName, object? value)
        {
            if (!optionName.Contains('['))
                Name = optionName;
            else //If the name contains '[]', parse out the number between brackets and use for array index of list config value.
            {
                var name = optionName[..optionName.IndexOf('[')];
                var extraIndex = optionName[(optionName.IndexOf('[') + 1)..];
                extraIndex = extraIndex.Remove(extraIndex.IndexOf(']'));

                Name = name;

                if (int.TryParse(extraIndex, out var result))
                    ArrayIndexer = result;
            }

            Value = value;
        }
    }
}
