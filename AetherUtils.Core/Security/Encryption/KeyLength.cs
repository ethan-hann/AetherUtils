namespace AetherUtils.Core.Security.Encryption
{
    /// <summary>
    /// Represents the valid key lengths (in bits) for the AES encryption algorithm.
    /// </summary>
    internal enum KeyLength
    {
        /// <summary>
        /// Specifies a key length of 128 bits (16 bytes)
        /// </summary>
        Bits128 = 16,
        /// <summary>
        /// Specifies a key length of 192 bits (24 bytes)
        /// </summary>
        Bits192 = 24,
        /// <summary>
        /// Specifies a key length of 256 bits (32 bytes)
        /// </summary>
        Bits256 = 32
    }
}
