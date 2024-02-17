using AetherUtils.Core.Security.Hashing;
using System.Security.Cryptography;

namespace AetherUtils.Core.Structs
{
    /// <summary>
    /// Represents the components of a hashed string.
    /// </summary>
    internal sealed class ParsedHash
    {
        /// <summary>
        /// The encoding of the hashed string.
        /// </summary>
        internal HashEncoding Encoding { get; private set; }
        
        /// <summary>
        /// The salt of the hashed string.
        /// </summary>
        internal byte[] Salt { get; private set; }
        
        /// <summary>
        /// The hash value of the hashed string.
        /// </summary>
        internal byte[] Hash { get; private set; }

        /// <summary>
        /// The number of iterations for the hashed string.
        /// </summary>
        internal int Iterations { get; private set; }

        /// <summary>
        /// The algorithm used to hash the string.
        /// </summary>
        internal HashAlgorithmName Algorithm { get; private set; }
        
        internal ParsedHash(HashEncoding encoding, byte[] salt, byte[] hash, int iterations, HashAlgorithmName algorithm)
        {
            Encoding = encoding;
            Salt = salt;
            Hash = hash;
            Iterations = iterations;
            Algorithm = algorithm;
        }
    }
}
