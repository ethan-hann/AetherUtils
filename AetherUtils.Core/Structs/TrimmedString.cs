namespace AetherUtils.Core.Structs
{
    /// <summary>
    /// Represents a string that has new line characters (\n) inserted and has been formatted to be a specific length.
    /// </summary>
    public readonly struct TrimmedString
    {
        /// <summary>
        /// The trimmed string.
        /// </summary>
        public readonly string String;

        /// <summary>
        /// The number of new line characters (\n) in the string.
        /// </summary>
        public readonly int Lines;

        /// <summary>
        /// Create a new <see cref="TrimmedString"/> struct from the specified string.
        /// </summary>
        /// <param name="s">The string to initialize the struct with.</param>
        public TrimmedString(string s)
        {
            String = s;
            Lines = String.Count(c => c.Equals('\n'));
        }
    }
}
