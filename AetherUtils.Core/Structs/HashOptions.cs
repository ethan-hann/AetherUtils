using AetherUtils.Core.Security.Hashing;
using System.Security.Cryptography;

namespace AetherUtils.Core.Structs
{
    /// <summary>
    /// Represents the options used for performing hashing functions. If unspecified at struct creation, 
    /// the <see cref="HashAlgorithm"/> defaults to <see cref="HashAlgorithmName.SHA384"/> 
    /// and the <see cref="Encoding"/> defaults to <see cref="HashEncoding.Base64"/>.
    /// <para>Once created, the properties cannot be changed.</para>
    /// </summary>
    public readonly struct HashOptions
    {
        private readonly ReadOnlyPair<int, int> _iterationsSpan;

        /// <summary>
        /// The length (in bytes) used for the salt when hashing.
        /// </summary>
        public int SaltLength { get; } = 16; //128 bits

        /// <summary>
        /// The size (in bytes) of the hash key used when hashing.
        /// </summary>
        public int KeySize { get; } = 48; //384 bits since we are using SHA384 by default.

        /// <summary>
        /// The number of iterations to perform when hashing.
        /// <para>A random value between the minimum and maximum iterations is retrieved every time this property is called.</para>
        /// </summary>
        public int Iterations => new Random().Next(_iterationsSpan.Key, _iterationsSpan.Value);

        /// <summary>
        /// The algorithm to use when hashing.
        /// </summary>
        public HashAlgorithmName HashAlgorithm { get; } = HashAlgorithmName.SHA384;

        /// <summary>
        /// The encoding scheme to use when hashing.
        /// </summary>
        public HashEncoding Encoding { get; } = HashEncoding.Base64;

        public HashOptions(ReadOnlyPair<int, int> iterations) => _iterationsSpan = iterations;

        public HashOptions(int minIterations, int maxIterations) => _iterationsSpan = new ReadOnlyPair<int, int>(minIterations, maxIterations);

        public HashOptions(int minIterations, int maxIterations, HashAlgorithmName hashAlgorithm, HashEncoding hashEncoding)
            : this(minIterations, maxIterations)
        { HashAlgorithm = hashAlgorithm; Encoding = hashEncoding; }

        public HashOptions(int minIterations, int maxIterations, HashAlgorithmName hashAlgorithm)
            : this(minIterations, maxIterations)
        { HashAlgorithm = hashAlgorithm; }

        public HashOptions(int minIterations, int maxIterations, HashEncoding hashEncoding)
            : this(minIterations, maxIterations)
        { Encoding = hashEncoding; }

        public HashOptions(ReadOnlyPair<int, int> iterations, HashEncoding hashEncoding)
            : this(iterations)
        { Encoding = hashEncoding; }

        public HashOptions(ReadOnlyPair<int, int> iterations, HashAlgorithmName hashAlgorithm, HashEncoding hashEncoding)
            : this(iterations)
        { HashAlgorithm = hashAlgorithm; Encoding = hashEncoding; }

        public HashOptions(int saltLength, ReadOnlyPair<int, int> iterations, HashAlgorithmName hashAlgorithm, HashEncoding hashEncoding)
            : this(iterations, hashAlgorithm, hashEncoding)
        { SaltLength = saltLength; }

        public HashOptions(int saltLength, ReadOnlyPair<int, int> iterations, HashAlgorithmName hashAlgorithm)
            : this(iterations)
        { SaltLength = saltLength; HashAlgorithm = hashAlgorithm; }

        public HashOptions(int saltLength, int minIterations, int maxIterations)
            : this(minIterations, maxIterations)
        { SaltLength = saltLength; }

        public HashOptions(int saltLength, int keySize, int minIterations, int maxIterations)
            : this(saltLength, minIterations, maxIterations)
        { KeySize = keySize; }

        public HashOptions(int saltLength, int keySize, int minIterations, int maxIterations, HashAlgorithmName hashAlgorithm)
            : this(saltLength, keySize, minIterations, maxIterations)
        { HashAlgorithm = hashAlgorithm; }

        public HashOptions(int saltLength, int keySize, int minIterations, int maxIterations, HashAlgorithmName hashAlgorithm, HashEncoding hashEncoding)
            : this(saltLength, keySize, minIterations, maxIterations, hashAlgorithm)
        { Encoding = hashEncoding; }
    }
}
