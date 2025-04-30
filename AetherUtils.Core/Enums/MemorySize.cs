// MemorySize.cs : AetherUtils
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
using JetBrains.Annotations;

namespace AetherUtils.Core.Enums;

/// <summary>
///     Represents the value in bytes of various memory sizes; uses <c>1024</c> as the conversion factor.
/// </summary>
internal enum MemorySize : ulong
{
    /// <summary>
    ///     Represents 1 Byte.
    /// </summary>
    [Description("Byte")]
    Byte = 1,

    /// <summary>
    ///     Represents 1 Kilobyte.
    /// </summary>
    [Description("KB")]
    KiloByte = Byte * 1024,

    /// <summary>
    ///     Represents 1 MegaByte.
    /// </summary>
    [Description("MB")]
    MegaByte = KiloByte * 1024,

    /// <summary>
    ///     Represents 1 GigaByte.
    /// </summary>
    [Description("GB")]
    GigaByte = MegaByte * 1024,

    /// <summary>
    ///     Represents 1 TerraByte.
    /// </summary>
    [Description("TB")]
    TerraByte = GigaByte * 1024,

    /// <summary>
    ///     Represents 1 PetaByte.
    /// </summary>
    [Description("PB")]
    PetaByte = TerraByte * 1024,

    /// <summary>
    ///     Represents 1 ExaByte.
    /// </summary>
    [Description("EB")]
    [UsedImplicitly]
    ExaByte = PetaByte * 1024
}