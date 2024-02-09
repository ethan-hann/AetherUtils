using System.Security.Cryptography;

namespace AetherUtils.Core.Security.Encryption;

/// <summary>
/// Provides methods to encrypt and decrypt <see cref="byte"/> arrays.
/// </summary>
public class ByteEncryptionService : EncryptionBase, IEncryptService<byte[], byte[]>
{
    public async Task<byte[]> EncryptAsync(byte[] input, string passphrase)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        ArgumentNullException.ThrowIfNull(passphrase, nameof(passphrase));

        using Aes aes = Aes.Create();
        aes.Key = DeriveKeyFromString(passphrase, 5000, KeyLength.Bits_256);

        await using MemoryStream output = new();
        WriteIvToStream(aes.IV, output);

        await using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
        await cryptoStream.WriteAsync(input);
        await cryptoStream.FlushFinalBlockAsync();

        return output.ToArray();
    }

    public async Task<byte[]> DecryptAsync(byte[] input, string passphrase)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        ArgumentNullException.ThrowIfNull(passphrase, nameof(passphrase));

        using Aes aes = Aes.Create();
        aes.Key = DeriveKeyFromString(passphrase, 5000, KeyLength.Bits_256);
        await using MemoryStream inputStream = new(input);
        aes.IV = ReadIvFromStream(inputStream);

        await using CryptoStream cryptoStream = new(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
        await using MemoryStream output = new();
        await cryptoStream.CopyToAsync(output);

        return output.ToArray();
    }
}