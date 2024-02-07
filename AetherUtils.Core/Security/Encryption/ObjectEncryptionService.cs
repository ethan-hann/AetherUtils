using AetherUtils.Core.Extensions;
using AetherUtils.Core.Files;

namespace AetherUtils.Core.Security.Encryption
{
    /// <summary>
    /// Provides methods to encrypt and decrypt XML serializable .NET objects.
    /// </summary>
    /// <typeparam name="T">The object type to encrypt/decrypt. This type must support XML serialization.</typeparam>
    public sealed class ObjectEncryptionService<T> : EncryptionBase, IEncryptService<T, byte[]> where T : class
    {
        private readonly StringEncryptionService _stringEncryptor = new();
        private readonly FileEncryptionService _fileEncryptionService = new();

        /// <summary>
        /// Encrypt a .NET XML serializable object to an encrypted <see cref="byte"/> array.
        /// </summary>
        /// <param name="input">The serializable .NET object.</param>
        /// <param name="passphrase">The passphrase for encryption.</param>
        /// <returns>An encrypted .NET object represented as a serialized <see cref="byte"/> array.</returns>
        /// <exception cref="InvalidOperationException">If the <paramref name="input"/> could not serialized to type <typeparamref name="T"/>.</exception>
        public async Task<byte[]> EncryptAsync(T input, string passphrase)
        {
            ArgumentNullException.ThrowIfNull(input, nameof(input));

            if (!input.CanSerialize())
                throw new InvalidOperationException($"{input.GetType()} does not support XML serialization.");

            string? objectString;

            if (input is string inputString)
                return await _stringEncryptor.EncryptAsync(inputString, passphrase);
            else
                objectString = input.Serialize();

            if (objectString is { } obj)
                return await _stringEncryptor.EncryptAsync(obj, passphrase);

            return [];
        }

        /// <summary>
        /// Decrypt a .NET object represented as an encrypted XML serialized <see cref="byte"/> array.
        /// </summary>
        /// <param name="input">The serialized, encrypted .NET object as a <see cref="byte"/> array.</param>
        /// <param name="passphrase">The passphrase for decryption.</param>
        /// <returns>The <typeparamref name="T"/> object, decrypted and deserialized.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="input"/> was <c>null</c> or 0-length.</exception>
        /// <exception cref="FormatException">If the <paramref name="input"/> could not be deserialized to type <typeparamref name="T"/>.</exception>
        public async Task<T> DecryptAsync(byte[] input, string passphrase)
        {
            if (input == null || input.Length == 0) throw new ArgumentNullException(nameof(input));

            var decrypted = await _stringEncryptor.DecryptAsync(input, passphrase);

            var result = decrypted.Deserialize<T>();
            
            if (result == null)
                throw new FormatException("The input was not in the correct format for decryption.");
            return result;
        }

        /// <summary>
        /// Encrypt a .NET XML serializable object to an encrypted file.
        /// </summary>
        /// <param name="input">The serializable .NET object.</param>
        /// <param name="filePath">The file to create (or overwrite).</param>
        /// <param name="passphrase">The passphrase for encryption.</param>
        /// <returns>An encrypted .NET object represented as a serialized <see cref="byte"/> array; or an empty <see cref="byte"/> array if the encryption failed.</returns>
        /// <exception cref="InvalidOperationException">If the <paramref name="input"/> could not serialized to type <typeparamref name="T"/>.</exception>
        public async Task<byte[]> EncryptToFileAsync(T input, string filePath, string passphrase)
        {
            ArgumentNullException.ThrowIfNull(input, nameof(input));
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            _fileEncryptionService.FilePath = filePath;

            if (!input.CanSerialize())
                throw new InvalidOperationException($"{input.GetType()} does not support XML serialization.");

            string? objectString;
            if (input is string inputString)
                return await _stringEncryptor.EncryptAsync(inputString, passphrase);
            else
                objectString = input.Serialize();

            var encryptedPath = string.Empty;
            if (objectString is { } obj)
                encryptedPath = await _fileEncryptionService.EncryptAsync(obj, passphrase);

            if (!encryptedPath.Equals(string.Empty))
                return await File.ReadAllBytesAsync(encryptedPath);

            return [];
        }

        /// <summary>
        /// Decrypt a .NET object from an encrypted file.
        /// </summary>
        /// <param name="filePath">The path to the file containing an encrypted, serialized .NET object.</param>
        /// <param name="passphrase">The passphrase for decryption.</param>
        /// <returns>The <typeparamref name="T"/> object, decrypted and deserialized.</returns>
        /// <exception cref="FileNotFoundException">If the file specified by <paramref name="filePath"/> did not exist.</exception>
        /// <exception cref="FormatException">If the object could not be deserialized to a new .NET object.</exception>
        public async Task<T> DecryptFromFileAsync(string filePath, string passphrase)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            filePath = FileHelper.ExpandPath(filePath);
            
            if (!FileHelper.DoesFileExist(filePath))
                throw new FileNotFoundException(nameof(filePath));

            

            var decryptedContents = await _fileEncryptionService.DecryptAsync(filePath, passphrase);

            T? result = decryptedContents.Deserialize<T>();
            if (result is not null)
                return result;

            throw new FormatException("The input was not in the correct format for decryption.");
        }
    }
}
