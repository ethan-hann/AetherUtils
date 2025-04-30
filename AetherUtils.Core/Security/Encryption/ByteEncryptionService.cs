// ByteEncryptionService.cs : AetherUtils
// Copyright (C) 2025  Ethan Hann
// 
// MIT License
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Security.Cryptography;

namespace AetherUtils.Core.Security.Encryption;

/// <summary>
///     Provides methods to encrypt and decrypt <see cref="byte" /> arrays.
/// </summary>
public sealed class ByteEncryptionService : EncryptionBase, IEncryptService<byte[], byte[]>
{
    /// <summary>
    ///     Encrypt the specified <see cref="byte" /> array with the specified passphrase.
    /// </summary>
    /// <param name="input">The <see cref="byte" /> array to encrypt.</param>
    /// <param name="passphrase">The passphrase used to derive the encryption key.</param>
    /// <returns>The encrypted <see cref="byte" /> array.</returns>
    public async Task<byte[]> EncryptAsync(byte[] input, string passphrase)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        ArgumentNullException.ThrowIfNull(passphrase, nameof(passphrase));

        using var aes = Aes.Create();
        aes.Key = DeriveKeyFromString(passphrase);

        await using MemoryStream output = new();
        WriteHeaderToStream(output);
        WriteIvToStream(aes.IV, output);

        await using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
        await cryptoStream.WriteAsync(input);
        await cryptoStream.FlushFinalBlockAsync();

        return output.ToArray();
    }

    /// <summary>
    ///     Decrypt the specified <see cref="byte" /> array with the specified passphrase.
    /// </summary>
    /// <param name="input">The encrypted <see cref="byte" /> array to decrypt.</param>
    /// <param name="passphrase">The passphrase used to derive the decryption key.</param>
    /// <returns>The decrypted <see cref="byte" /> array.</returns>
    public async Task<byte[]> DecryptAsync(byte[] input, string passphrase)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        ArgumentNullException.ThrowIfNull(passphrase, nameof(passphrase));

        using var aes = Aes.Create();
        aes.Key = DeriveKeyFromString(passphrase);
        await using MemoryStream inputStream = new(input);
        RemoveHeaderFromStream(inputStream);
        aes.IV = ReadIvFromStream(inputStream);

        await using CryptoStream cryptoStream = new(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
        await using MemoryStream output = new();
        await cryptoStream.CopyToAsync(output);

        return output.ToArray();
    }
}