// // IEncryptService.cs : AetherUtils
// // Copyright (C) 2025  Ethan Hann
// //
// // MIT License
// // Permission is hereby granted, free of charge, to any person obtaining a copy
// // of this software and associated documentation files (the "Software"), to deal
// // in the Software without restriction, including without limitation the rights
// // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// // copies of the Software, and to permit persons to whom the Software is
// // furnished to do so, subject to the following conditions:
// //
// // The above copyright notice and this permission notice shall be included in all
// // copies or substantial portions of the Software.
// //
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// // SOFTWARE.

using JetBrains.Annotations;

namespace AetherUtils.Core.Security.Encryption;

/// <summary>
///     Represents an interface for encryption classes to implement.
///     Provides generic methods for encryption and decryption of data.
/// </summary>
/// <typeparam name="TInput">The input type for encryption.</typeparam>
/// <typeparam name="TOutput">The output type after decryption.</typeparam>
internal interface IEncryptService<TInput, TOutput>
{
    /// <summary>
    ///     Encrypt the <typeparamref name="TInput" />  <paramref name="input" /> using the specified passphrase.
    /// </summary>
    /// <param name="input">The data to encrypt.</param>
    /// <param name="passphrase">A passphrase used to derive the encryption key.</param>
    /// <returns>The <typeparamref name="TOutput" /> encrypted data.</returns>
    [UsedImplicitly]
    public Task<TOutput> EncryptAsync(TInput input, string passphrase);

    /// <summary>
    ///     Decrypt the <typeparamref name="TOutput" /> <paramref name="input" /> using the specified passphrase.
    /// </summary>
    /// <param name="input">The data to decrypt.</param>
    /// <param name="passphrase">The passphrase used to derive the decryption key.</param>
    /// <returns>The <typeparamref name="TInput" /> decrypted data.</returns>
    [UsedImplicitly]
    public Task<TInput> DecryptAsync(TOutput input, string passphrase);
}