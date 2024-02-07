using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AetherUtils.Core.Extensions;

namespace AetherUtils.Core.Security.Encryption
{
    /// <summary>
    /// Provides methods to encrypt and decrypt XML serializable .NET objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectEncryptionService<T> : EncryptionBase, IEncryptService<T, byte[]> where T : class
    {
        private readonly StringEncryptionService _stringEncryptor;

        public ObjectEncryptionService()
        {
            _stringEncryptor = new();
        }

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

            if (objectString is string obj)
                return await _stringEncryptor.EncryptAsync(obj, passphrase);

            return [];
        }

        public async Task<T> DecryptAsync(byte[] input, string passphrase)
        {
            if (input == null || input.Length == 0) throw new ArgumentNullException(nameof(input));

            var decrypted = await _stringEncryptor.DecryptAsync(input, passphrase);

            T? result = decrypted.Deserialize<T>();
            if (result is not null)
                return result;

            throw new FormatException("The input was not in the correct format for decryption.");
        }
    }
}
