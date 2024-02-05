using AetherUtils.Core.Utility;
using AetherUtils.Core.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AetherUtils.Core.Extensions;

namespace AetherUtils.Core.Security
{
    //Implemented based on: https://stackoverflow.com/questions/2138429/hash-and-salt-passwords-in-c-sharp/73126492#73126492

    /// <summary>
    /// Provides a service for creating cryptographically strong hashes of <see cref="string"/>s and comparing plain-text <see cref="string"/>s against them for equality.
    /// <para>This class hashes strings based on the provided <see cref="HashOptions"/>. Once this service has been created and used,
    /// the hashed string cannot be changed and a new <see cref="HashService"/> must be created.</para>
    /// </summary>
    public class HashService
    {
        private readonly HashOptions _options;

        private readonly Random _random = new((int)DateTime.Now.Ticks);

        private string _hashedString = string.Empty;

        private const char _delimiter = ':';

        /// <summary>
        /// Get the hashed string for this <see cref="HashService"/>.
        /// </summary>
        public string HashedString => _hashedString;

        private HashService() { }

        /// <summary>
        /// Create a new service with the specified <see cref="HashOptions"/>.
        /// </summary>
        /// <param name="options">The options that should be used for hashing.</param>
        public HashService(HashOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Generate a hash string for the specified plain-text string according to this object's <see cref="HashOptions"/>.
        /// </summary>
        /// <param name="value">The plain text string to hash.</param>
        /// <returns>A cryptographically strong hashed string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> was <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if this method has already been called and generated a valid hashed string previously.</exception>
        public string HashString(string value)
        {
            ArgumentNullException.ThrowIfNull(nameof(value));

            byte[] salt = RandomNumberGenerator.GetBytes(_options.SaltLength);
            int iterations = _options.Iterations;

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                value,
                salt,
                iterations,
                _options.HashAlgorithm,
                _options.KeySize
            );

            _hashedString = string.Join(
                _delimiter,
                _options.Encoding,
                _options.Encoding == HashEncoding.Hex ? Convert.ToHexString(hash) : Convert.ToBase64String(hash),
                _options.Encoding == HashEncoding.Hex ? Convert.ToHexString(salt) : Convert.ToBase64String(salt),
                iterations,
                _options.HashAlgorithm
            );
            return _hashedString;
        }

        /// <summary>
        /// Parse a hashed string into its <see cref="ParsedHash"/> equivalent.
        /// </summary>
        /// <param name="hashString"></param>
        /// <returns></returns>
        private static ParsedHash ParseHashedString(string hashString)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(hashString));

            string[] segments = hashString.Split(_delimiter);
            HashEncoding encoding = Enum.Parse<HashEncoding>(segments[0]);
            byte[] hash = segments[1].DecodedBytesFromEncodedString(encoding);
            byte[] salt = segments[2].DecodedBytesFromEncodedString(encoding);
            int iterations = int.Parse(segments[3]);
            HashAlgorithmName algorithm = new(segments[4]);

            return new ParsedHash(encoding, salt, hash, iterations, algorithm);
        }

        /// <summary>
        /// Compare a non-hashed input <see cref="string"/> against the last hashed string for this <see cref="HashService"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <returns><c>true</c> if the two strings are equivalent; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="input"/> is <c>null</c> or empty.</exception>
        public bool CompareHash(string input) => CompareHash(input, _hashedString);

        /// <summary>
        /// Compare a non-hashed input <see cref="string"/> against a hashed <see cref="string"/>.
        /// </summary>
        /// <param name="input">The non-hashed string.</param>
        /// <param name="hashString">A hash string to compare against.</param>
        /// <returns><c>true</c> if the two strings are equivalent; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="input"/> or <paramref name="hashString"/> are <c>null</c> or empty.</exception>
        public static bool CompareHash(string input, string hashString)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(input));
            ArgumentException.ThrowIfNullOrEmpty(nameof(hashString));

            ParsedHash p = ParseHashedString(hashString);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                p.Salt,
                p.Iterations,
                p.Algorithm,
                p.Hash.Length
            );
            return CryptographicOperations.FixedTimeEquals(inputHash, p.Hash);
        }
    }
}
