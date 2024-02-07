using AetherUtils.Core.Security.Hashing;
using System.Security.Cryptography;

namespace AetherUtils.Core.Structs
{
    internal class ParsedHash
    {
        internal HashEncoding Encoding { get; private set; }
        internal byte[] Salt { get; private set; }
        internal byte[] Hash { get; private set; }

        internal int Iterations { get; private set; }

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
