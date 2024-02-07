using System.Security.Cryptography;
using System.Text;

namespace AetherUtils.Core.Security.Encryption
{
    /// <summary>
    /// Represents the base class for all encryption service classes. All encryption is performed using AES-256 and encoded with <see cref="Encoding.UTF32"/>.
    /// </summary>
    public class EncryptionBase
    {
        /// <summary>
        /// Reads the first 16 bytes from an encrypted <see cref="Stream"/>. The first 16 bytes should be the IV.
        /// </summary>
        /// <param name="encryptedData">The encrypted data to read.</param>
        /// <param name="stream">The stream to read from.</param>
        /// <returns></returns>
        internal byte[] ReadIvFromStream(Stream stream)
        {
            var iv = new byte[16];
            _ = stream.Read(iv, 0, 16);
            return iv;
        }

        /// <summary>
        /// Writes an initialization vector to the first 16 bytes of a <see cref="Stream"/>.
        /// </summary>
        /// <param name="iv">The IV to write.</param>
        /// <param name="stream">The stream to write to.</param>
        internal void WriteIvToStream(byte[] iv, Stream stream) => stream.Write(iv, 0, 16);

        /// <summary>
        /// Derives a key to use for encryption/decryption from an input string.
        /// <para>This method should be called with the same arguments when encrypting and decrypting. Failure to do so will
        /// result in a failed decryption based on different keys.</para>
        /// </summary>
        /// <param name="input">A plain-text string to derive key from.</param>
        /// <param name="iterations">The number of iterations. (Default is 5000).</param>
        /// <param name="keyLength">The length of the generated key, in bytes.</param>
        /// <returns>A cryptographically strong key.</returns>
        internal byte[] DeriveKeyFromString(string input, int iterations = 5000, KeyLength keyLength = KeyLength.Bits_128)
        {
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF32.GetBytes(input),
                Array.Empty<byte>(),
                iterations,
                HashAlgorithmName.SHA384,
                (int)keyLength);
        }

        /// <summary>
        /// Get a cryptographically strong random key that can be used for encryption.
        /// </summary>
        /// <param name="keySize">The size, in bytes, for the generated key.</param>
        /// <returns>A cryptographically strong <see cref="byte"/> array.</returns>
        private static byte[] GetRandomKey(int keySize = 32)
        {
            var bytes = new byte[keySize];
            RandomNumberGenerator.Create().GetBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// Get a cryptographically strong random key phrase that can be used for encryption.
        /// </summary>
        /// <param name="keySize">The size, in bytes, for the generated key.</param>
        /// <returns>A cryptographically strong <see cref="string"/>.</returns>
        public static string GetRandomKeyPhrase(int keySize = 32)
        {
            var bytes = GetRandomKey(keySize);
            return GetStringFromUtf32(bytes);
        }

        /// <summary>
        /// Get a <see cref="string"/> equalvalent to the <paramref name="input"/> in <see cref="Encoding.UTF32"/> encoding.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A <see cref="string"/> representing the <paramref name="input"/> in <see cref="Encoding.UTF32"/>.</returns>
        internal static string GetStringFromUtf32(byte[] input) => Encoding.UTF32.GetString(input);

        /// <summary>
        /// Get a <see cref="byte"/> array equivalent to the <paramref name="input"/> in <see cref="Encoding.UTF32"/> encoding.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A <see cref="byte"/> array representing the <paramref name="input"/> in <see cref="Encoding.UTF32"/> encoding.</returns>
        internal static byte[] GetBytesToUtf32(string input) => Encoding.UTF32.GetBytes(input);
    }
}
