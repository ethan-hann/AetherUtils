using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Licensing
{
    /// <summary>
    /// Stores a read-only pair of keys.
    /// </summary>
    /// <param name="privateKey">The private key.</param>
    /// <param name="publicKey">The public key.</param>
    public readonly struct KeyPair(string privateKey, string publicKey)
    {
        public string PrivateKey { get; } = privateKey;
        public string PublicKey { get; } = publicKey;
    }
}
