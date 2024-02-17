namespace AetherUtils.Core.Structs
{
    /// <summary>
    /// Represents a string that has new line characters (\n) inserted and has been formatted to be a specific length.
    /// <para>This struct contains the string and the number of new line characters added.</para>
    /// </summary>
    public readonly struct TrimmedString
    {
        /// <summary>
        /// The trimmed string.
        /// </summary>
        public string String { get; }

        /// <summary>
        /// The number of new line characters (\n) in the string.
        /// </summary>
        public int Lines { get; }

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
