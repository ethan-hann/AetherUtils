using AetherUtils.Core.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Security
{
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
        /// <para>This method can only be called once. Subsequent calls will result in an <see cref="InvalidOperationException"/>.</para>
        /// </summary>
        /// <param name="value">The plain text string to hash.</param>
        /// <returns>A cryptographically strong hashed string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> was <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if this method has already been called and generated a valid hashed string previously.</exception>
        public string HashString(string value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (!_hashedString.Equals(string.Empty))
                throw new InvalidOperationException("A hashed string has already been created on this instance.");

            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[_options.SaltLength];
            rng.GetBytes(salt);

            int iterations = _random.Next(_options.Iterations.Key, _options.Iterations.Value);
            Rfc2898DeriveBytes hashTool = new(value, _options.SaltLength, 
                iterations, _options.HashAlgorithm);

            byte[] hash = hashTool.GetBytes(_options.SaltLength);

            _hashedString = string.Format("{0}:{1}:{2}", iterations, Convert.ToBase64String(salt), Convert.ToBase64String(hash));

            return _hashedString;
        }

        /// <summary>
        /// Compares the provided <paramref name="value"/> against this object's <see cref="HashedString"/> to verify they are the same.
        /// </summary>
        /// <param name="value">The plain-text value to check against the hashed string.</param>
        /// <returns><c>true</c> if the two values are equivalent; <c>false</c> otherwise.</returns>
        /// <exception cref="InvalidOperationException">Thrown if there is not a valid <see cref="HashedString"/> created.</exception>
        public bool CompareHash(string value)
        {
            if (_hashedString == null || _hashedString.Equals(string.Empty)) 
                throw new InvalidOperationException("There is not a hashed string defined to compare against.");
            
            try
            {
                string[] hashParts = _hashedString.Split(':');
                if (int.TryParse(hashParts[0], out int iterations))
                {
                    byte[] originalSalt = Convert.FromBase64String(hashParts[1]);
                    byte[] originalHash = Convert.FromBase64String(hashParts[2]);

                    Rfc2898DeriveBytes hashTool = new(value, originalSalt, iterations, _options.HashAlgorithm);
                    byte[] testHash = hashTool.GetBytes(originalHash.Length);

                    //Compare the two strings (hashed and non-hashed)
                    var differences = Convert.ToUInt32(originalHash.Length) ^ Convert.ToUInt32(testHash.Length);

                    //Iterate through the differences and ensure that each bit is equal
                    for (var position = 0; position <= Math.Min(originalHash.Length, testHash.Length) - 1; position++)
                        differences |= Convert.ToUInt32(originalHash[position] ^ testHash[position]);

                    return differences == 0;
                }
                return false;
            } catch (Exception ex) { Debug.WriteLine(ex.Message); return false; }
        }
    }
}
