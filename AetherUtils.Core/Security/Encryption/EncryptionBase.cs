using System.Security.Cryptography;
using System.Text;
using AetherUtils.Core.Structs;

namespace AetherUtils.Core.Security.Encryption
{
    /// <summary>
    /// Represents the base class for all encryption service classes. All encryption is performed using the AES-256
    /// algorithm and encoded according to <see cref="Encoding.UTF8"/>.
    /// </summary>
    public class EncryptionBase
    {
        private const char SHGFI_EXTENSION_SEPARATOR = ':';

        /// <summary>
        /// Reads the first 16 bytes from an encrypted <see cref="Stream"/>. The first 16 bytes should be the IV.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns></returns>
        internal static byte[] ReadIvFromStream(Stream stream)
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
        internal static void WriteIvToStream(byte[] iv, Stream stream) => stream.Write(iv, 0, 16);

        /// <summary>
        /// Derives a key to use for encryption/decryption from an input string.
        /// <para>This method should be called with the same arguments when encrypting and decrypting. Failure to do so will
        /// result in a failed decryption based on different keys.</para>
        /// </summary>
        /// <param name="input">A plain-text string to derive key from.</param>
        /// <param name="iterations">The number of iterations. (Default is 5000).</param>
        /// <param name="keyLength">The length of the generated key, in bytes.</param>
        /// <returns>A cryptographically strong key.</returns>
        internal static byte[] DeriveKeyFromString(string input, int iterations = 5000, KeyLength keyLength = KeyLength.Bits256)
        {
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(input),
                Array.Empty<byte>(),
                iterations,
                HashAlgorithmName.SHA384,
                (int)keyLength);
        }

        /// <summary>
        /// Get a cryptographically strong random key that can be used for encryption.
        /// </summary>
        /// <param name="keySize">The size, in bytes, for the generated key.</param>
        /// <returns>A cryptographically strong and random <see cref="byte"/> array.</returns>
        public static byte[] GetRandomKey(int keySize = 32)
        {
            var bytes = new byte[keySize];
            RandomNumberGenerator.Create().GetBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// Get a cryptographically strong random key phrase that can be used for encryption.
        /// </summary>
        /// <param name="keySize">The size, in bytes, for the generated key.</param>
        /// <returns>A cryptographically strong and random <see cref="string"/>.</returns>
        public static string GetRandomKeyPhrase(int keySize = 32)
        {
            var bytes = GetRandomKey(keySize);
            return GetStringFromUtf8Bytes(bytes);
        }

        /// <summary>
        /// Write the <see cref="byte"/> representation of the <paramref name="extension"/> to the supplied buffer.
        /// <para>This method writes the extension bytes at the end of the original <paramref name="buffer"/>.</para>
        /// </summary>
        /// <param name="extension">The file extension to write, including the <c>period (.)</c>.</param>
        /// <param name="buffer">The buffer of bytes to write to, passed by reference.</param>
        /// <exception cref="ArgumentException">If <paramref name="extension"/> was <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="buffer"/> was <c>null</c>.</exception>
        internal static void WriteExtensionToBytes(string extension, ref byte[] buffer)
        {
            ArgumentException.ThrowIfNullOrEmpty(extension, nameof(extension));
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
            
            using MemoryStream inputStream = new(buffer);
            using MemoryStream outputStream = new();
            inputStream.CopyTo(outputStream);
            var extensionBytes = Encoding.UTF8.GetBytes($"{SHGFI_EXTENSION_SEPARATOR}{extension}{SHGFI_EXTENSION_SEPARATOR}");
            outputStream.WriteAsync(extensionBytes, 0, extensionBytes.Length);
            
            buffer = outputStream.ToArray();
        }

        /// <summary>
        /// Get the original extension string from a <see cref="byte"/> buffer
        /// that was written to with <see cref="WriteExtensionToBytes"/>. This method modifies the buffer and removes
        /// the extension bytes from it.
        /// </summary>
        /// <param name="buffer">The buffer of bytes to read the extension from, passed by reference.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If <paramref name="buffer"/> was <c>null</c>.</exception>
        internal static string GetExtensionFromBytes(ref byte[] buffer)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
            
            //Find the first and last index of the separator char from the byte array.
            ReadOnlyPair<int, int> separatorIndices = FindSeparatorIndices(buffer);
            
            //Get the bytes for the extension using the first and last index found above.
            var extensionBytes = buffer[separatorIndices.Key..(separatorIndices.Value + 1)];
            
            //Create a new buffer array with the correct length (the extension bytes truncated)
            var newBuffer = new byte[buffer.Length - extensionBytes.Length];
            
            //Copy the correct number of bytes from the original buffer into the new buffer.
            //This is a crucial step otherwise we will get padding errors on decryption.
            Array.Copy(buffer, newBuffer, newBuffer.Length);
            buffer = newBuffer; //Finally, assign the new buffer back to the original buffer.

            return GetExtensionString(extensionBytes);
        }

        /// <summary>
        /// Find the first and last indices of the separator character in the specified <see cref="byte"/> array.
        /// </summary>
        /// <param name="buffer">The <see cref="byte"/> array to search.</param>
        /// <returns>A <see cref="ReadOnlyPair{K,V}"/> where the <c>key</c> is the first index
        /// and the <c>value</c> is the last index.</returns>
        private static ReadOnlyPair<int, int> FindSeparatorIndices(IReadOnlyList<byte> buffer)
        {
            var foundLastIndex = false;
            var foundFirstIndex = false;
            var lastIndexSeparator = 0;
            var firstIndexSeparator = 0;
            
            for (int i = buffer.Count - 1; i > 0; i--)
            {
                if ((char)buffer[i] != SHGFI_EXTENSION_SEPARATOR) continue;
                
                if (!foundLastIndex)
                {
                    lastIndexSeparator = i;
                    foundLastIndex = true;
                }
                else if (!foundFirstIndex)
                {
                    firstIndexSeparator = i;
                    foundFirstIndex = true;
                }
            }

            return new ReadOnlyPair<int, int>(firstIndexSeparator, lastIndexSeparator);
        }

        /// <summary>
        /// Get the extension string from the <see cref="byte"/> array.
        /// </summary>
        /// <param name="buffer">An array of bytes representing an extension.</param>
        /// <returns>A string representation of the extension.</returns>
        /// <exception cref="NullReferenceException">If the extension could not be read from the supplied
        /// <see cref="byte"/> array.</exception>
        private static string GetExtensionString(byte[] buffer)
        {
            var byteString = Encoding.UTF8.GetString(buffer);
            
            var separatorIndex = byteString.IndexOf(SHGFI_EXTENSION_SEPARATOR);
            var extension = byteString.TakeLast(byteString.Length - separatorIndex);
            var sb = new StringBuilder();
            var enumerable = extension as char[] ?? extension.ToArray();
            enumerable.ToList().ForEach(c => sb = sb.Append(c));
            if (extension is null)
                throw new NullReferenceException("Extension was null after reading from bytes.");
            
            var extensionString = sb.ToString();
            extensionString = extensionString.Substring(extensionString.IndexOf(SHGFI_EXTENSION_SEPARATOR) + 1,
                extensionString.LastIndexOf(SHGFI_EXTENSION_SEPARATOR) - 1);
            return extensionString;
        }

        /// <summary>
        /// Get a <see cref="string"/> equalvalent to the <paramref name="input"/> in <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A <see cref="string"/> representing the <paramref name="input"/> in <see cref="Encoding.UTF8"/>.</returns>
        internal static string GetStringFromUtf8Bytes(byte[] input) => Encoding.UTF8.GetString(input);

        /// <summary>
        /// Get a <see cref="byte"/> array equivalent to the <paramref name="input"/> in <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A <see cref="byte"/> array representing the <paramref name="input"/> in <see cref="Encoding.UTF8"/> encoding.</returns>
        internal static byte[] GetBytesFromUtf8String(string input) => Encoding.UTF8.GetBytes(input);
    }
}
