using AetherUtils.Core.Structs;

namespace AetherUtils.Core.Licensing
{
    /// <summary>
    /// Stores a read-only pair of keys.
    /// </summary>
    /// <param name="privateKey">The private key.</param>
    /// <param name="publicKey">The public key.</param>
    public sealed class KeyPair(string publicKey, string privateKey)
    {
        /// <summary>
        /// The public key component of the key pair.
        /// </summary>
        public string PublicKey { get; } = publicKey;
        
        /// <summary>
        /// The private key component of the key pair.
        /// </summary>
        public string PrivateKey { get; } = privateKey;

        /// <summary>
        /// Get a read-only pair containing the public and private key.
        /// </summary>
        public ReadOnlyPair<string, string> AsReadOnlyPair => new(PublicKey, PrivateKey);
    }
}
