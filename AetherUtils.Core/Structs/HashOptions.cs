using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Structs
{
    /// <summary>
    /// Represents the options used for performing hashing functions. If unspecified at object creation, 
    /// the <see cref="HashAlgorithm"/> defaults to <see cref="HashAlgorithmName.SHA256"/>.
    /// </summary>
    public readonly struct HashOptions
    {
        /// <summary>
        /// The length used for the salt when hashing.
        /// </summary>
        public readonly short SaltLength { get; }

        /// <summary>
        /// The number of minimum (key) and maximum (value) iterations to perform when hashing.
        /// </summary>
        public readonly ReadOnlyPair<short, short> Iterations { get; }

        public readonly HashAlgorithmName HashAlgorithm { get; }

        /// <summary>
        /// Create a new HashOptions with the specified salt length and iterations pair.
        /// </summary>
        /// <param name="saltLength"></param>
        /// <param name="iterations">A key-value pair where: <c>key = minimum iterations</c> and <c>value = maximum iterations.</c></param>
        public HashOptions(short saltLength,  ReadOnlyPair<short, short> iterations)
        {
            SaltLength = saltLength;
            Iterations = iterations;
            HashAlgorithm = HashAlgorithmName.SHA256;
        }

        /// <summary>
        /// Create a new HashOptions with the specified salt length, minimum iterations, and maximum iterations.
        /// </summary>
        /// <param name="saltLength"></param>
        /// <param name="minIterations"></param>
        /// <param name="maxIterations"></param>
        public HashOptions(short saltLength, short minIterations, short maxIterations)
            : this(saltLength, new ReadOnlyPair<short, short>(minIterations, maxIterations)) { }

        /// <summary>
        /// Create a new HashOptions with the specified salt length, iterations, and hash algorithm.
        /// </summary>
        /// <param name="saltLength"></param>
        /// <param name="iterations"></param>
        /// <param name="hashAlgorithm"></param>
        public HashOptions(short saltLength, ReadOnlyPair<short, short> iterations, HashAlgorithmName hashAlgorithm)
            : this(saltLength, iterations)
        {
            HashAlgorithm = hashAlgorithm;
        }

        /// <summary>
        /// Create a new HashOptions with the specified salt length, iterations, and hash algorithm.
        /// </summary>
        /// <param name="saltLength"></param>
        /// <param name="minIterations"></param>
        /// <param name="maxIterations"></param>
        /// <param name="hashAlgorithm"></param>
        public HashOptions(short saltLength, short minIterations, short maxIterations, HashAlgorithmName hashAlgorithm) 
            : this(saltLength, minIterations, maxIterations)
        {
            HashAlgorithm = hashAlgorithm;
        }
    }
}
