// KeyPair.cs : AetherUtils
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

using System.ComponentModel;
using AetherUtils.Core.Structs;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AetherUtils.Core.Licensing;

/// <summary>
///     Stores a read-only pair of keys.
/// </summary>
/// <param name="privateKey">The private key.</param>
/// <param name="publicKey">The public key.</param>
public sealed class KeyPair(string publicKey, string privateKey)
{
    /// <summary>
    ///     Create a new key pair with the specified public and private keys as well as the passphrase.
    /// </summary>
    /// <param name="publicKey">The public key.</param>
    /// <param name="privateKey">The private key.</param>
    /// <param name="passphrase">The passphrase used to derive the keys.</param>
    [JsonConstructor]
    public KeyPair(string publicKey, string privateKey, string passphrase)
        : this(publicKey, privateKey)
    {
        Passphrase = passphrase;
    }

    /// <summary>
    ///     The public key component of the key pair.
    /// </summary>
    [Browsable(true)]
    [Description("The public key for this key pair.")]
    [Category("Keys")]
    [UsedImplicitly]
    public string PublicKey { get; } = publicKey;

    /// <summary>
    ///     The private key component of the key pair.
    /// </summary>
    [Browsable(true)]
    [Description("The private key for this key pair.")]
    [Category("Keys")]
    [UsedImplicitly]
    public string PrivateKey { get; } = privateKey;

    /// <summary>
    ///     The passphrase used to generate the keys.
    /// </summary>
    [Browsable(false)]
    public string Passphrase { [UsedImplicitly] get; } = string.Empty;

    /// <summary>
    ///     Get a read-only pair containing the public and private key.
    /// </summary>
    [Browsable(false)]
    [JsonIgnore]
    [UsedImplicitly]
    public ReadOnlyPair<string, string> AsReadOnlyPair => new(PublicKey, PrivateKey);
}