// // TrimmedString.cs : AetherUtils
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
///     Represents a string that has new line characters (\n) inserted and has been formatted to be a specific length.
///     <para>This struct contains the string and the number of new line characters added.</para>
/// </summary>
public readonly struct TrimmedString
{
    /// <summary>
    ///     The trimmed string.
    /// </summary>
    [UsedImplicitly]
    public string String { get; }

    /// <summary>
    ///     The number of new line characters (\n) in the string.
    /// </summary>
    public int Lines { [UsedImplicitly] get; }

    /// <summary>
    ///     Create a new <see cref="TrimmedString" /> struct from the specified string.
    /// </summary>
    /// <param name="s">The string to initialize the struct with.</param>
    public TrimmedString(string s)
    {
        String = s;
        Lines = String.Count(c => c.Equals('\n'));
    }
}