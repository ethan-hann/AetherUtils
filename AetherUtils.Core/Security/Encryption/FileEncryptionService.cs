using AetherUtils.Core.Files;
using System.Security.Cryptography;

namespace AetherUtils.Core.Security.Encryption
{
    /// <summary>
    /// Provides methods to encrypt and decrypt files on the file system.
    /// </summary>
    public sealed class FileEncryptionService : EncryptionBase, IEncryptService<string, string>
    {
        private static ByteEncryptionService _byteEncryptor = new();
        
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
        /// Create and encrypt a file with the specified <paramref name="content"/> using the <paramref name="passphrase"/>.
        /// <para>This method will either create a new file if one does not exist or overwrite the existing file.</para>
        /// </summary>
        /// <param name="content">The string to encrypt into the file.</param>
        /// <param name="passphrase">The passphrase used for encryption.</param>
        /// <returns>The path to the encrypted file.</returns>
        /// <exception cref="ArgumentException">If <paramref name="content"/> was <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="passphrase"/> was <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">If the <see cref="FilePath"/> property is empty.</exception>
        public async Task<string> EncryptAsync(string content, string passphrase)
        {
            ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
            ArgumentNullException.ThrowIfNull(passphrase, nameof(passphrase));

            if (FilePath.Equals(string.Empty))
                throw new InvalidOperationException("File path was empty.");
            
            FilePath = FileHelper.ExpandPath(FilePath);
            FileHelper.CreateDirectories(FilePath);
            
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromString(passphrase, 5000, KeyLength.Bits_256);
            await using FileStream output = new(FilePath, FileMode.Create);
            WriteIvToStream(aes.IV, output);

            await using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await cryptoStream.WriteAsync(GetBytesFromUtf8String(content));
            await cryptoStream.FlushFinalBlockAsync();

            return FilePath;
        }

        public static async Task<string> EncryptFileAsync(string filePath, string passphrase)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            ArgumentNullException.ThrowIfNull(passphrase, nameof(passphrase));

            filePath = FileHelper.ExpandPath(filePath);
            if (!FileHelper.DoesFileExist(filePath, false))
                 throw new FileNotFoundException("The file was not found.", filePath);

            var contents = await FileHelper.OpenNonTextFileAsync(filePath, false);
            var extension = FileHelper.GetExtension(filePath, false);
            var encryptedContents = _byteEncryptor.EncryptAsync(contents, passphrase).Result;
            
            WriteExtensionToBytes(extension, ref encryptedContents);

            var oldFilePath = filePath;
            
            filePath = Path.ChangeExtension(filePath, ".enc"); //change extension on path file to encrypted file extension.
            FileHelper.SaveFileAsync(filePath, encryptedContents, false); //save the encrypted file.
            FileHelper.DeleteFile(oldFilePath, false); //delete the original file.

            return filePath;
        }
        
        public static async Task<string> DecryptFileAsync(string filePath, string passphrase)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            ArgumentNullException.ThrowIfNull(passphrase, nameof(passphrase));

            filePath = FileHelper.ExpandPath(filePath);
            if (!FileHelper.DoesFileExist(filePath, false))
                throw new FileNotFoundException("The file was not found.", filePath);

            var contents = await FileHelper.OpenNonTextFileAsync(filePath, false);
            var extension = GetExtensionFromBytes(ref contents);
            var decryptedContents = _byteEncryptor.DecryptAsync(contents, passphrase);

            var oldFilePath = filePath;
            
            filePath = Path.ChangeExtension(filePath, extension);
            FileHelper.SaveFileAsync(filePath, decryptedContents.Result, false); //save the decrypted file.
            FileHelper.DeleteFile(oldFilePath, false); //delete the encrypted file.
            
            return filePath;
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

            return GetStringFromUtf8Bytes(output.ToArray());
        }
    }
}
