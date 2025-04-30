// HashOptions.cs : AetherUtils
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
using AetherUtils.Core.Security.Hashing;
using JetBrains.Annotations;

namespace AetherUtils.Core.Structs;

/// <summary>
///     Represents the options used for performing hashing functions. If unspecified at struct creation,
///     the <see cref="HashAlgorithm" /> defaults to <see cref="HashAlgorithmName.SHA384" />
///     and the <see cref="Encoding" /> defaults to <see cref="HashEncoding.Base64" />.
///     <para>Once created, the properties cannot be changed.</para>
/// </summary>
public readonly struct HashOptions
{
    private readonly ReadOnlyPair<int, int> _iterationsSpan;

    /// <summary>
    ///     The length (in bytes) used for the salt when hashing.
    /// </summary>
    public int SaltLength { get; } = 16; //128 bits

    /// <summary>
    ///     The size (in bytes) of the hash key used when hashing.
    /// </summary>
    public int KeySize { get; } = 48; //384 bits since we are using SHA384 by default.

    /// <summary>
    ///     The number of iterations to perform when hashing.
    ///     <para>
    ///         A cryptographically strong random value between the minimum and maximum iterations is
    ///         retrieved every time this property is retrieved.
    ///     </para>
    /// </summary>
    public int Iterations => Math.Abs(RandomNumberGenerator.GetInt32(_iterationsSpan.Key, _iterationsSpan.Value));

    /// <summary>
    ///     The algorithm to use when hashing.
    /// </summary>
    public HashAlgorithmName HashAlgorithm { get; } = HashAlgorithmName.SHA384;

    /// <summary>
    ///     The encoding scheme to use when hashing.
    /// </summary>
    public HashEncoding Encoding { get; } = HashEncoding.Base64;

    /// <summary>
    ///     Create a new hash options specifying the iterations.
    /// </summary>
    /// <param name="iterations"></param>
    [UsedImplicitly]
    public HashOptions(ReadOnlyPair<int, int> iterations)
    {
        _iterationsSpan = iterations;
    }

    /// <summary>
    ///     Create a new hash options specifying the minimum and maximum iterations seperately.
    /// </summary>
    /// <param name="minIterations"></param>
    /// <param name="maxIterations"></param>
    [UsedImplicitly]
    public HashOptions(int minIterations, int maxIterations)
    {
        _iterationsSpan = new ReadOnlyPair<int, int>(minIterations, maxIterations);
    }

    /// <summary>
    ///     Create a new hash options specifying the minimum and maximum iterations, the hash algorithm, and the hash encoding.
    /// </summary>
    /// <param name="minIterations"></param>
    /// <param name="maxIterations"></param>
    /// <param name="hashAlgorithm"></param>
    /// <param name="hashEncoding"></param>
    [UsedImplicitly]
    public HashOptions(int minIterations, int maxIterations, HashAlgorithmName hashAlgorithm, HashEncoding hashEncoding)
        : this(minIterations, maxIterations)
    {
        HashAlgorithm = hashAlgorithm;
        Encoding = hashEncoding;
    }

    /// <summary>
    ///     Create a new hash options specifying the minimum and maximum iterations and the hash algorithm.
    /// </summary>
    /// <param name="minIterations"></param>
    /// <param name="maxIterations"></param>
    /// <param name="hashAlgorithm"></param>
    [UsedImplicitly]
    public HashOptions(int minIterations, int maxIterations, HashAlgorithmName hashAlgorithm)
        : this(minIterations, maxIterations)
    {
        HashAlgorithm = hashAlgorithm;
    }

    /// <summary>
    ///     Create a new hash options specifying the minimum and maximum iterations and the hash encoding.
    /// </summary>
    /// <param name="minIterations"></param>
    /// <param name="maxIterations"></param>
    /// <param name="hashEncoding"></param>
    [UsedImplicitly]
    public HashOptions(int minIterations, int maxIterations, HashEncoding hashEncoding)
        : this(minIterations, maxIterations)
    {
        Encoding = hashEncoding;
    }

    /// <summary>
    ///     Create a new hash options specifying the iterations and the hash encoding.
    /// </summary>
    /// <param name="iterations"></param>
    /// <param name="hashEncoding"></param>
    [UsedImplicitly]
    public HashOptions(ReadOnlyPair<int, int> iterations, HashEncoding hashEncoding)
        : this(iterations)
    {
        Encoding = hashEncoding;
    }

    /// <summary>
    ///     Create a new hash options specifying the iterations, the hash algorithm, and the hash encoding.
    /// </summary>
    /// <param name="iterations"></param>
    /// <param name="hashAlgorithm"></param>
    /// <param name="hashEncoding"></param>
    [UsedImplicitly]
    public HashOptions(ReadOnlyPair<int, int> iterations, HashAlgorithmName hashAlgorithm, HashEncoding hashEncoding)
        : this(iterations)
    {
        HashAlgorithm = hashAlgorithm;
        Encoding = hashEncoding;
    }

    /// <summary>
    ///     Create a new hash options specifying the salt length, iterations, the hash algorithm, and the hash encoding.
    /// </summary>
    /// <param name="saltLength"></param>
    /// <param name="iterations"></param>
    /// <param name="hashAlgorithm"></param>
    /// <param name="hashEncoding"></param>
    [UsedImplicitly]
    public HashOptions(int saltLength, ReadOnlyPair<int, int> iterations, HashAlgorithmName hashAlgorithm, HashEncoding hashEncoding)
        : this(iterations, hashAlgorithm, hashEncoding)
    {
        SaltLength = saltLength;
    }

    /// <summary>
    ///     Create a new hash options specifying the salt length, the iterations, and the hash algorithm.
    /// </summary>
    /// <param name="saltLength"></param>
    /// <param name="iterations"></param>
    /// <param name="hashAlgorithm"></param>
    [UsedImplicitly]
    public HashOptions(int saltLength, ReadOnlyPair<int, int> iterations, HashAlgorithmName hashAlgorithm)
        : this(iterations)
    {
        SaltLength = saltLength;
        HashAlgorithm = hashAlgorithm;
    }

    /// <summary>
    ///     Create a new hash options specifying the salt length and the minimum and maximum iterations.
    /// </summary>
    /// <param name="saltLength"></param>
    /// <param name="minIterations"></param>
    /// <param name="maxIterations"></param>
    [UsedImplicitly]
    public HashOptions(int saltLength, int minIterations, int maxIterations)
        : this(minIterations, maxIterations)
    {
        SaltLength = saltLength;
    }

    /// <summary>
    ///     Create a new hash options specifying the salt length, the key size, and the minimum and maximum iterations.
    /// </summary>
    /// <param name="saltLength"></param>
    /// <param name="keySize"></param>
    /// <param name="minIterations"></param>
    /// <param name="maxIterations"></param>
    [UsedImplicitly]
    public HashOptions(int saltLength, int keySize, int minIterations, int maxIterations)
        : this(saltLength, minIterations, maxIterations)
    {
        KeySize = keySize;
    }

    /// <summary>
    ///     Create a new hash options specifying the salt length, the key size, the minimum and maximum iterations, and the
    ///     hash algorithm.
    /// </summary>
    /// <param name="saltLength"></param>
    /// <param name="keySize"></param>
    /// <param name="minIterations"></param>
    /// <param name="maxIterations"></param>
    /// <param name="hashAlgorithm"></param>
    [UsedImplicitly]
    public HashOptions(int saltLength, int keySize, int minIterations, int maxIterations, HashAlgorithmName hashAlgorithm)
        : this(saltLength, keySize, minIterations, maxIterations)
    {
        HashAlgorithm = hashAlgorithm;
    }

    /// <summary>
    ///     Create a new hash options specifying the salt length, the key size, the minimum and maximum iterations,
    ///     the hash algorithm, and the hash encoding.
    /// </summary>
    /// <param name="saltLength"></param>
    /// <param name="keySize"></param>
    /// <param name="minIterations"></param>
    /// <param name="maxIterations"></param>
    /// <param name="hashAlgorithm"></param>
    /// <param name="hashEncoding"></param>
    [UsedImplicitly]
    public HashOptions(int saltLength, int keySize, int minIterations, int maxIterations, HashAlgorithmName hashAlgorithm, HashEncoding hashEncoding)
        : this(saltLength, keySize, minIterations, maxIterations, hashAlgorithm)
    {
        Encoding = hashEncoding;
    }
}