using AetherUtils.Core.Files;
using System.Security.Cryptography;

namespace AetherUtils.Core.Security.Encryption
{
    /// <summary>
    /// Provides methods to encrypt and decrypt files on the file system.
    /// </summary>
    public sealed class FileEncryptionService : EncryptionBase, IEncryptService<string, string>
    {
        /// <summary>
        /// Get or set the file path that is used for encryption.
        /// </summary>
        public string FilePath { get; set; }

        public FileEncryptionService(string filePath)
        {
            FilePath = FileHelper.ExpandPath(filePath);
            FileHelper.CreateDirectories(FilePath, false);
        }

        internal FileEncryptionService() => FilePath = string.Empty;

        /// <summary>
        /// Encrypt a file with the specified <paramref name="content"/> using the <paramref name="passphrase"/>.
        /// <para>This method will either create a new file if one does not exist or overwrite the existing file.</para>
        /// </summary>
        /// <param name="content">The string to encrypt into the file.</param>
        /// <param name="passphrase">The passphrase used for encryption.</param>
        /// <returns>The path to the encrypted file.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="FilePath"/> property is empty.</exception>
        public async Task<string> EncryptAsync(string content, string passphrase)
        {
            ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

            if (FilePath.Equals(string.Empty))
                throw new InvalidOperationException("File path was empty.");
            
            FilePath = FileHelper.ExpandPath(FilePath);
            FileHelper.CreateDirectories(FilePath);
            
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromString(passphrase, 5000, KeyLength.Bits_256);
            await using FileStream output = new(FilePath, FileMode.Create);
            WriteIvToStream(aes.IV, output);

            await using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await cryptoStream.WriteAsync(GetBytesToUtf32(content));
            await cryptoStream.FlushFinalBlockAsync();

            return FilePath;
        }

        /// <summary>
        /// Decrypt a file specified by <paramref name="filePath"/> using the <paramref name="passphrase"/>.
        /// </summary>
        /// <param name="filePath">The path to the file to decrypt.</param>
        /// <param name="passphrase">The passphrase used for decryption.</param>
        /// <returns>The decrypted contents of the file.</returns>
        /// <exception cref="FileNotFoundException">If <paramref name="filePath"/> was not a valid path to a file.</exception>
        /// <exception cref="ArgumentException"> If <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public async Task<string> DecryptAsync(string filePath, string passphrase)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            if (!FileHelper.DoesFileExist(filePath))
                throw new FileNotFoundException($"File {filePath} was not found.");
            
            filePath = FileHelper.ExpandPath(filePath);
            
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromString(passphrase, 5000, KeyLength.Bits_256);
            await using FileStream inputStream = new(filePath, FileMode.Open);
            aes.IV = ReadIvFromStream(inputStream);

            await using CryptoStream cryptoStream = new(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();
            await cryptoStream.CopyToAsync(output);

            return GetStringFromUtf32(output.ToArray());
        }
    }
}
