using System.Security.Cryptography;

namespace AetherUtils.Core.Security.Encryption;

/// <summary>
/// Provides methods to encrypt and decrypt <see cref="byte"/> arrays.
/// </summary>
public sealed class ByteEncryptionService : EncryptionBase, IEncryptService<byte[], byte[]>
{
    /// <summary>
    /// Encrypt the specified <see cref="byte"/> array with the specified passphrase.
    /// </summary>
    /// <param name="input">The <see cref="byte"/> array to encrypt.</param>
    /// <param name="passphrase">The passphrase used to derive the encryption key.</param>
    /// <returns>The encrypted <see cref="byte"/> array.</returns>
    public async Task<byte[]> EncryptAsync(byte[] input, string passphrase)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        ArgumentNullException.ThrowIfNull(passphrase, nameof(passphrase));

        using Aes aes = Aes.Create();
        aes.Key = DeriveKeyFromString(passphrase);

        await using MemoryStream output = new();
        WriteIvToStream(aes.IV, output);

        await using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
        await cryptoStream.WriteAsync(input);
        await cryptoStream.FlushFinalBlockAsync();

        return output.ToArray();
    }

    /// <summary>
    /// Decrypt the specified <see cref="byte"/> array with the specified passphrase.
    /// </summary>
    /// <param name="input">The encrypted <see cref="byte"/> array to decrypt.</param>
    /// <param name="passphrase">The passphrase used to derive the decryption key.</param>
    /// <returns>The decrypted <see cref="byte"/> array.</returns>
    public async Task<byte[]> DecryptAsync(byte[] input, string passphrase)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        ArgumentNullException.ThrowIfNull(passphrase, nameof(passphrase));

        using Aes aes = Aes.Create();
        aes.Key = DeriveKeyFromString(passphrase);
        await using MemoryStream inputStream = new(input);
        aes.IV = ReadIvFromStream(inputStream);

        await using CryptoStream cryptoStream = new(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
        await using MemoryStream output = new();
        await cryptoStream.CopyToAsync(output);

        return output.ToArray();
    }
}