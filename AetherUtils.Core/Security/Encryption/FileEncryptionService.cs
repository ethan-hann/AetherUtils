using AetherUtils.Core.Files;
using System.Security.Cryptography;

namespace AetherUtils.Core.Security.Encryption
{
    public class FileEncryptionService : EncryptionBase, IEncryptService<string, string>
    {
        public string FilePath { get; set; } = string.Empty;

        public FileEncryptionService(string filePath)
        {
            FilePath = FileHelper.ExpandPath(filePath);
        }

        public FileEncryptionService() { }

        /// <summary>
        /// Encrypt a file with the specified <paramref name="content"/> using the <paramref name="passphrase"/>.
        /// <para>This method will either create a new file if one does not exist or overwrite the existing file.</para>
        /// </summary>
        /// <param name="content">The string to encrypt into the file.</param>
        /// <param name="passphrase">The passphrase used for encryption.</param>
        /// <exception cref="InvalidOperationException">If the <see cref="FilePath"/> property is empty.</exception>
        /// <returns>The path to the encrypted file.</returns>
        public async Task<string> EncryptAsync(string content, string passphrase)
        {
            if (FilePath.Equals(string.Empty))
                throw new InvalidOperationException("File Path cannot be empty.");

            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromString(passphrase, 5000, KeyLength.Bits_256);
            using FileStream output = new(FilePath, FileMode.Create);
            WriteIVToStream(aes.IV, output);

            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await cryptoStream.WriteAsync(GetBytesToUTF32(content));
            await cryptoStream.FlushFinalBlockAsync();

            return FilePath;
        }

        /// <summary>
        /// Decrypt a file specified by <paramref name="filePath"/> using the <paramref name="passphrase"/>.
        /// </summary>
        /// <param name="filePath">The path to the file to decrypt.</param>
        /// <param name="passphrase">The passphrase used for decryption.</param>
        /// <returns>The decrypted contents of the file.</returns>
        public async Task<string> DecryptAsync(string filePath, string passphrase)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromString(passphrase, 5000, KeyLength.Bits_256);
            using FileStream inputStream = new(filePath, FileMode.Open);
            aes.IV = ReadIVFromStream(inputStream);

            using CryptoStream cryptoStream = new(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();
            await cryptoStream.CopyToAsync(output);

            return GetStringFromUTF32(output.ToArray());
        }
    }
}
