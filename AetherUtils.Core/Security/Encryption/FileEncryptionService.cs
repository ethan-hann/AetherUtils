using AetherUtils.Core.Files;
using System.Security.Cryptography;

namespace AetherUtils.Core.Security.Encryption
{
    /// <summary>
    /// Provides methods to encrypt and decrypt files on the file system.
    /// <remarks>This service contains potentially destructive methods that can render a file unreadable for a few reasons:
    /// <list type="number">
    ///     <item>If a file is encrypted with <see cref="EncryptFileAsync"/> and then opened in a text editor
    ///             (to view/edit the contents),
    ///             and then the file is saved, it could render the file unable to be decrypted.</item>
    ///     <item>If after performing item 1, you attempt to encrypt the file again, it could be lost forever.</item>
    ///     <item>If the file extension of an encrypted file is changed manually, the file will be unable to be decrypted.</item>
    /// </list>
    /// <b>Use at your own risk!</b>
    /// </remarks>
    /// </summary>
    public sealed class FileEncryptionService : EncryptionBase, IEncryptService<string, string>
    {
        private static readonly ByteEncryptionService ByteEncryptor = new();
        
        /// <summary>
        /// Get or set the file path that is used for encryption.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Create a new file encryption service using the specified file path.
        /// </summary>
        /// <param name="filePath">The file path associated with this service.</param>
        public FileEncryptionService(string filePath)
        {
            FilePath = FileHelper.ExpandPath(filePath);
            FileHelper.CreateDirectories(FilePath, false);
        }

        internal FileEncryptionService() => FilePath = string.Empty;

        /// <summary>
        /// Get a value indicating if the specified file is encrypted.
        /// </summary>
        /// <param name="filePath">The full path to the file.</param>
        /// <returns><c>true</c> if the file is encrypted; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentException">If <paramref name="filePath"/> is <c>null</c> or empty.</exception>
        public static bool IsEncryptedFile(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            try
            {
                using FileStream inputStream = new(filePath, FileMode.Open);
                return ContainsHeaderBytes(inputStream);
            }
            catch (CryptographicException) { return false; }
        }

        /// <summary>
        /// Create a file with the specified <paramref name="content"/> and encrypt the contents
        /// using the <paramref name="passphrase"/>.
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
            aes.Key = DeriveKeyFromString(passphrase);
            await using FileStream output = new(FilePath, FileMode.Create);
            WriteIvToStream(aes.IV, output);

            await using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await cryptoStream.WriteAsync(GetBytesFromUtf8String(content));
            await cryptoStream.FlushFinalBlockAsync();

            return FilePath;
        }
        
        /// <summary>
        /// Decrypt a file's contents specified by <paramref name="filePath"/> using the <paramref name="passphrase"/>.
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
                throw new FileNotFoundException("The file was not found.", filePath);
            
            filePath = FileHelper.ExpandPath(filePath);
            
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromString(passphrase);
            await using FileStream inputStream = new(filePath, FileMode.Open);
            aes.IV = ReadIvFromStream(inputStream);

            await using CryptoStream cryptoStream = new(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();
            await cryptoStream.CopyToAsync(output);

            return GetStringFromUtf8Bytes(output.ToArray());
        }

        /// <summary>
        /// Encrypt an existing file on disk using the specified <paramref name="passphrase"/> to derive the key.
        /// </summary>
        /// <remarks>If the original file's extension is different than the encrypted file's,
        /// the original file is deleted after encryption occurs and the file extension is
        /// changed to the new extension.</remarks>
        /// <param name="filePath">The path to the file on disk to encrypt.</param>
        /// <param name="passphrase">The passphrase used for encryption.</param>
        /// <param name="newExtension">The file extensions to change the encrypted file to.
        /// If no file extension change is wanted, supply the original file's extension; default is <c>.enc</c>.</param>
        /// <returns>The path to the encrypted file.</returns>
        /// <exception cref="FileNotFoundException">If the file could not be found at the <paramref name="filePath"/>.</exception>
        public static async Task<string> EncryptFileAsync(string filePath, string passphrase, string newExtension = ".enc")
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            ArgumentNullException.ThrowIfNull(passphrase, nameof(passphrase));

            filePath = FileHelper.ExpandPath(filePath);
            if (!FileHelper.DoesFileExist(filePath, false))
                 throw new FileNotFoundException("The file was not found.", filePath);

            var contents = await FileHelper.OpenNonTextFileAsync(filePath, false);
            var extension = FileHelper.GetExtension(filePath, false);
            var encryptedContents = ByteEncryptor.EncryptAsync(contents, passphrase).Result;
            
            WriteExtensionToBytes(extension, ref encryptedContents);

            var oldFilePath = filePath;
            
            filePath = Path.ChangeExtension(filePath, newExtension); //change extension on path file to encrypted file extension.
            FileHelper.SaveFileAsync(filePath, encryptedContents, false); //save the encrypted file.
            
            if (!FileHelper.GetExtension(oldFilePath, false).Equals(newExtension))
                FileHelper.DeleteFile(oldFilePath, false); //delete the original file if the extensions are different.

            return filePath;
        }
        
        /// <summary>
        /// Decrypt an encrypted file from disk using the specified <paramref name="passphrase"/> to derive the key.
        /// </summary>
        /// <remarks>If the encrypted file's extension is different than the original file's,
        /// the encrypted file is deleted after decryption occurs and the file extension is
        /// changed back to the original extension.</remarks>
        /// <param name="filePath">The path to the file on disk to decrypt.</param>
        /// <param name="passphrase">The passphrase used for decryption.</param>
        /// <returns>The path to the decrypted file.</returns>
        /// <exception cref="FileNotFoundException">If the file could not be found at the <paramref name="filePath"/>.</exception>
        public static async Task<string> DecryptFileAsync(string filePath, string passphrase)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            ArgumentNullException.ThrowIfNull(passphrase, nameof(passphrase));

            filePath = FileHelper.ExpandPath(filePath);
            if (!FileHelper.DoesFileExist(filePath, false))
                throw new FileNotFoundException("The file was not found.", filePath);

            var contents = await FileHelper.OpenNonTextFileAsync(filePath, false);
            var extension = GetExtensionFromBytes(ref contents);
            var decryptedContents = ByteEncryptor.DecryptAsync(contents, passphrase);

            var oldFilePath = filePath;
            
            filePath = Path.ChangeExtension(filePath, extension);
            FileHelper.SaveFileAsync(filePath, decryptedContents.Result, false); //save the decrypted file.
            
            if (!FileHelper.GetExtension(oldFilePath, false).Equals(extension))
                FileHelper.DeleteFile(oldFilePath, false); //delete the encrypted file if the extensions are different.
            
            return filePath;
        }
    }
}
