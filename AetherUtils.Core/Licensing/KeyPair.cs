namespace AetherUtils.Core.Licensing
{
    /// <summary>
    /// Stores a read-only pair of keys.
    /// </summary>
    /// <param name="privateKey">The private key.</param>
    /// <param name="publicKey">The public key.</param>
    public readonly struct KeyPair(string privateKey, string publicKey)
    {
        /// <summary>
        /// The private key component of the key pair.
        /// </summary>
        public string PrivateKey { get; } = privateKey;
        
        /// <summary>
        /// The public key component of the key pair.
        /// </summary>
        public string PublicKey { get; } = publicKey;
    }
}
