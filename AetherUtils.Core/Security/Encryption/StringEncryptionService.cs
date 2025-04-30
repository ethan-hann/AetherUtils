// StringEncryptionService.cs : AetherUtils
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
///     Provides methods to encrypt and decrypt strings.
/// </summary>
public sealed class StringEncryptionService : EncryptionBase, IEncryptService<string, byte[]>
{
    /// <summary>
    ///     Encrypt a string using the <paramref name="passphrase" />.
    /// </summary>
    /// <param name="input">The string to encrypt.</param>
    /// <param name="passphrase">The passphrase used for encryption.</param>
    /// <returns>The encrypted <see cref="byte" /> array.</returns>
    public async Task<byte[]> EncryptAsync(string input, string passphrase)
    {
        using var aes = Aes.Create();
        aes.Key = DeriveKeyFromString(passphrase);
        await using MemoryStream output = new();
        WriteIvToStream(aes.IV, output);

        await using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
        await cryptoStream.WriteAsync(GetBytesFromUtf8String(input));
        await cryptoStream.FlushFinalBlockAsync();

        return output.ToArray();
    }

    /// <summary>
    ///     Decrypt an encrypted <see cref="byte" /> array using the <paramref name="passphrase" />.
    /// </summary>
    /// <param name="encrypted">An encrypted <see cref="byte" /> array.</param>
    /// <param name="passphrase">The passphrase used for decryption.</param>
    /// <returns>The decrypted <see cref="string" />.</returns>
    public async Task<string> DecryptAsync(byte[] encrypted, string passphrase)
    {
        using var aes = Aes.Create();
        aes.Key = DeriveKeyFromString(passphrase);

        using var input = new MemoryStream(encrypted);
        aes.IV = ReadIvFromStream(input);

        await using CryptoStream cs = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
        await using MemoryStream output = new();
        await cs.CopyToAsync(output);

        return GetStringFromUtf8Bytes(output.ToArray());
    }
}