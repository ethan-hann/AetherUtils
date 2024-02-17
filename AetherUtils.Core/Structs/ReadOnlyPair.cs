namespace AetherUtils.Core.Structs
{
    /// <summary>
    /// Represents a generic key-value pair that is read-only; Once the pair is instantiated, the values cannot be changed.
    /// </summary>
    /// <typeparam name="TK">The <see cref="Type"/> for the key.</typeparam>
    /// <typeparam name="TV">The <see cref="Type"/> for the value.</typeparam>
    public readonly struct ReadOnlyPair<TK, TV>(TK? key, TV? value)
    {
        /// <summary>
        /// The key component of this pair.
        /// </summary>
        public readonly TK? Key = key;
        
        /// <summary>
        /// The value component of this pair.
        /// </summary>
        public readonly TV? Value = value;
    }
}
