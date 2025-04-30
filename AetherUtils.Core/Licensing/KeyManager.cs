// // KeyManager.cs : AetherUtils
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

using AetherUtils.Core.Extensions;
using AetherUtils.Core.Files;
using AetherUtils.Core.Structs;
using JetBrains.Annotations;
using Standard.Licensing.Security.Cryptography;

namespace AetherUtils.Core.Licensing;

/// <summary>
///     Handles loading and saving private and public keys to file system.
/// </summary>
[UsedImplicitly]
public static class KeyManager
{
    /// <summary>
    ///     Create a new public and private key pair using the specified passphrase.
    /// </summary>
    /// <param name="passphrase">The passphrase used to derive the key pair.</param>
    /// <returns>A new <see cref="KeyPair" /> containing a public and private key.</returns>
    [UsedImplicitly]
    public static KeyPair CreateKeys(string passphrase)
    {
        var generator = KeyGenerator.Create();
        var keyPair = generator.GenerateKeyPair();
        var privateKey = keyPair.ToEncryptedPrivateKeyString(passphrase);
        var publicKey = keyPair.ToPublicKeyString();

        return new KeyPair(publicKey, privateKey);
    }

    /// <summary>
    ///     Save all the key pair files specified in the list to disk. Each pair in the list should be in this format:<br />
    ///     <c>Full Path, Key Pair</c>
    /// </summary>
    /// <param name="pathsAndKeys">A list of paths and <see cref="KeyPair" /> objects to save.</param>
    [UsedImplicitly]
    public static void SaveAllKeys(IEnumerable<Pair<string, KeyPair>> pathsAndKeys)
    {
        foreach (var pair in pathsAndKeys)
            SaveKeys(pair.Value, pair.Key);
    }

    /// <summary>
    ///     Save the full key pair to a file.
    /// </summary>
    /// <param name="keys">The pair of keys (public and private). Public key should be first.</param>
    /// <param name="fullPath">The full path of the file to save.</param>
    /// <exception cref="ArgumentException">If <paramref name="fullPath" /> was <c>null</c> or empty.</exception>
    /// <exception cref="ArgumentException">If <paramref name="keys" /> was <c>null</c>.</exception>
    [UsedImplicitly]
    public static void SaveKeys(KeyPair keys, string fullPath)
    {
        ArgumentNullException.ThrowIfNull(keys, nameof(keys));
        ArgumentException.ThrowIfNullOrEmpty(fullPath, nameof(fullPath));

        var json = keys.SerializeJson();
        FileHelper.CreateDirectories(fullPath);
        FileHelper.SaveFile(fullPath, json);
    }

    /// <summary>
    ///     Save the full key pair to a file.
    /// </summary>
    /// <param name="keys">The pair of keys (public and private). Public key should be first.</param>
    /// <param name="basePath">The base folder to save the keys in.</param>
    /// <param name="fileName">The name of the file to save: recommended is <c>keyPair.keys</c></param>
    /// <exception cref="ArgumentException">If <paramref name="basePath" /> was <c>null</c> or empty.</exception>
    /// <exception cref="ArgumentException">If <paramref name="fileName" /> was <c>null</c> or empty.</exception>
    /// <exception cref="ArgumentException">If <paramref name="keys" /> was <c>null</c>.</exception>
    [UsedImplicitly]
    public static void SaveKeys(KeyPair keys, string basePath, string fileName)
    {
        var fullPath = Path.Combine(basePath, fileName);
        SaveKeys(keys, fullPath);
    }

    /// <summary>
    ///     Save only the public key to a file.
    /// </summary>
    /// <param name="publicKey">The public key to save.</param>
    /// <param name="fullPath">The full path of the file to save.</param>
    /// <exception cref="ArgumentException">If <paramref name="fullPath" /> was <c>null</c> or empty.</exception>
    /// <exception cref="ArgumentException">If <paramref name="publicKey" /> was <c>null</c> or empty.</exception>
    [UsedImplicitly]
    public static void SavePublicKey(string publicKey, string fullPath)
    {
        ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));
        ArgumentException.ThrowIfNullOrEmpty(fullPath, nameof(fullPath));

        var json = publicKey.SerializeJson();
        FileHelper.CreateDirectories(fullPath);
        FileHelper.SaveFile(fullPath, json);
    }

    /// <summary>
    ///     Save only the public key to a file.
    /// </summary>
    /// <param name="publicKey">The public key to save.</param>
    /// <param name="basePath">The base folder to save the public key in.</param>
    /// <param name="fileName">The name of the file to save: recommended is <c>public.key</c></param>
    /// <exception cref="ArgumentException">If <paramref name="basePath" /> was <c>null</c> or empty.</exception>
    /// <exception cref="ArgumentException">If <paramref name="fileName" /> was <c>null</c> or empty.</exception>
    /// <exception cref="ArgumentException">If <paramref name="publicKey" /> was <c>null</c> or empty.</exception>
    [UsedImplicitly]
    public static void SavePublicKey(string publicKey, string basePath, string fileName)
    {
        var fullPath = Path.Combine(basePath, fileName);
        SavePublicKey(publicKey, fullPath);
    }

    /// <summary>
    ///     Loads all key pairs specified by the list of full paths.
    /// </summary>
    /// <param name="paths">A list of full file paths to key files.</param>
    /// <returns>
    ///     A list of <see cref="KeyPair" />s loaded from the file paths.
    ///     This list is in the same order as the paths.
    /// </returns>
    [UsedImplicitly]
    public static List<KeyPair> LoadAllKeys(IEnumerable<string> paths)
    {
        List<KeyPair> pairs = [];
        pairs.AddRange(paths.Select(LoadKeys).OfType<KeyPair>());
        return pairs;
    }

    /// <summary>
    ///     Load a key pair from a file.
    /// </summary>
    /// <param name="basePath">The base folder the keys are located in.</param>
    /// <param name="fileName">The name of the file to load.</param>
    /// <returns>A <see cref="KeyPair" /> representing the key values.</returns>
    /// <exception cref="ArgumentException">If <paramref name="basePath" /> was <c>null</c> or empty.</exception>
    /// <exception cref="ArgumentException">If <paramref name="fileName" /> was <c>null</c> or empty.</exception>
    [UsedImplicitly]
    public static KeyPair? LoadKeys(string basePath, string fileName)
    {
        ArgumentException.ThrowIfNullOrEmpty(basePath, nameof(basePath));
        ArgumentException.ThrowIfNullOrEmpty(fileName, nameof(fileName));

        var fullPath = Path.Combine(basePath, fileName);
        return LoadKeys(fullPath);
    }

    /// <summary>
    ///     Load a key pair from a file.
    /// </summary>
    /// <param name="fullPath">The full, absolute path to a key file.</param>
    /// <returns>A <see cref="KeyPair" /> representing the key values or <c>null</c> if the file was not valid.</returns>
    /// <exception cref="ArgumentException">If <paramref name="fullPath" /> was <c>null</c> or empty.</exception>
    [UsedImplicitly]
    public static KeyPair? LoadKeys(string fullPath)
    {
        ArgumentException.ThrowIfNullOrEmpty(fullPath, nameof(fullPath));

        var json = FileHelper.OpenFile(fullPath);
        return new Json<KeyPair>().FromJson(json);
    }
}