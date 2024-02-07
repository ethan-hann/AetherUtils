using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Security.Encryption
{
    /// <summary>
    /// Provides methods to encrypt and decrypt strings.
    /// </summary>
    public class StringEncryptionService : EncryptionBase, IEncryptService<string, byte[]>
    {
        public async Task<byte[]> EncryptAsync(string input, string passphrase)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromString(passphrase, 5000, KeyLength.Bits_256);
            using MemoryStream output = new();
            WriteIVToStream(aes.IV, output);

            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await cryptoStream.WriteAsync(GetBytesToUTF32(input));
            await cryptoStream.FlushFinalBlockAsync();
            
            return output.ToArray();
        }

        public async Task<string> DecryptAsync(byte[] encrypted, string passphrase)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromString(passphrase, 5000, KeyLength.Bits_256);

            using MemoryStream input = new MemoryStream(encrypted);
            aes.IV = ReadIVFromStream(input);

            using CryptoStream cs = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();
            await cs.CopyToAsync(output);

            return GetStringFromUTF32(output.ToArray());
        }
    }
}
