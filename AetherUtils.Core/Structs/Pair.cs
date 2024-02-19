namespace AetherUtils.Core.Structs
{
    /// <summary>
    /// Represents a generic key-value pair.
    /// </summary>
    /// <typeparam name="TK">The <see cref="Type"/> for the key.</typeparam>
    /// <typeparam name="TV">The <see cref="Type"/> for the value.</typeparam>
    public struct Pair<TK, TV>(TK key, TV value) where TK : notnull where TV : notnull
    {
        /// <summary>
        /// The key component of this pair.
        /// </summary>
        public TK Key { get; set; } = key;
        
        /// <summary>
        /// The value component of this pair.
        /// </summary>
        public TV Value { get; set; } = value;
    }
}
