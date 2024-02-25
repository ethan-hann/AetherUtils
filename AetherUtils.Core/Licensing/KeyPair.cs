using System.ComponentModel;
using AetherUtils.Core.Structs;
using Newtonsoft.Json;

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
        [Browsable(true)]
        [Description("The public key for this key pair.")]
        [Category("Keys")]
        public string PublicKey { get; } = publicKey;
        
        /// <summary>
        /// The private key component of the key pair.
        /// </summary>
        [Browsable(true)]
        [Description("The private key for this key pair.")]
        [Category("Keys")]
        public string PrivateKey { get; } = privateKey;

        /// <summary>
        /// The passphrase used to generate the keys.
        /// </summary>
        [Browsable(false)]
        public string Passphrase { get; } = string.Empty;
        
        /// <summary>
        /// Get a read-only pair containing the public and private key.
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public ReadOnlyPair<string, string> AsReadOnlyPair => new(PublicKey, PrivateKey);
        
        /// <summary>
        /// Create a new key pair with the specified public and private keys as well as the passphrase.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="privateKey">The private key.</param>
        /// <param name="passphrase">The passphrase used to derive the keys.</param>
        [JsonConstructor]
        public KeyPair(string publicKey, string privateKey, string passphrase) 
            : this(publicKey, privateKey) => Passphrase = passphrase;
    }
}
