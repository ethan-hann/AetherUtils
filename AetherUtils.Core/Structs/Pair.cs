// // Pair.cs : AetherUtils
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

namespace AetherUtils.Core.Structs;

/// <summary>
///     Represents a generic key-value pair.
/// </summary>
/// <typeparam name="TK">The <see cref="Type" /> for the key.</typeparam>
/// <typeparam name="TV">The <see cref="Type" /> for the value.</typeparam>
public struct Pair<TK, TV>(TK key, TV value) where TK : notnull where TV : notnull
{
    /// <summary>
    ///     The key component of this pair.
    /// </summary>
    public TK Key { get; [UsedImplicitly] set; } = key;

    /// <summary>
    ///     The value component of this pair.
    /// </summary>
    public TV Value { get; [UsedImplicitly] set; } = value;
}