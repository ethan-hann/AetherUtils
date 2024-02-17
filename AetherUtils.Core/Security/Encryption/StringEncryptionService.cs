using System.Security.Cryptography;

namespace AetherUtils.Core.Security.Encryption
{
    /// <summary>
    /// Provides methods to encrypt and decrypt strings.
    /// </summary>
    public sealed class StringEncryptionService : EncryptionBase, IEncryptService<string, byte[]>
    {
        /// <summary>
        /// Encrypt a string using the <paramref name="passphrase"/>.
        /// </summary>
        /// <param name="input">The string to encrypt.</param>
        /// <param name="passphrase">The passphrase used for encryption.</param>
        /// <returns>The encrypted <see cref="byte"/> array.</returns>
        public async Task<byte[]> EncryptAsync(string input, string passphrase)
        {
            using var aes = Aes.Create();
            aes.Key = DeriveKeyFromString(passphrase);
            await using MemoryStream output = new();
            WriteIvToStream(aes.IV, output);

            await using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await cryptoStream.WriteAsync(GetBytesFromUtf8String(input));
            await cryptoStream.FlushFinalBlockAsync();

            return output.ToArray();
        }

        /// <summary>
        /// Decrypt an encrypted <see cref="byte"/> array using the <paramref name="passphrase"/>.
        /// </summary>
        /// <param name="encrypted">An encrypted <see cref="byte"/> array.</param>
        /// <param name="passphrase">The passphrase used for decryption.</param>
        /// <returns>The decrypted <see cref="string"/>.</returns>
        public async Task<string> DecryptAsync(byte[] encrypted, string passphrase)
        {
            using var aes = Aes.Create();
            aes.Key = DeriveKeyFromString(passphrase);

            using var input = new MemoryStream(encrypted);
            aes.IV = ReadIvFromStream(input);

            await using CryptoStream cs = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
            await using MemoryStream output = new();
            await cs.CopyToAsync(output);

            return GetStringFromUtf8Bytes(output.ToArray());
        }
    }
}
