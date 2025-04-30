// NumberExtensions.cs : AetherUtils
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

using AetherUtils.Core.Enums;
using JetBrains.Annotations;

namespace AetherUtils.Core.Extensions;

/// <summary>
///     Provides extension methods for manipulating various number objects.
/// </summary>
public static class NumberExtensions
{
    /// <summary>
    ///     Get an absolute value in inches represented by the specified number of pixels.
    /// </summary>
    /// <param name="pixels">The number of pixels to convert.</param>
    /// <param name="dpi">The DPI (dots per inch) of the screen; defaults to <c>96</c>.</param>
    /// <returns>The inches equivalent of the pixels.</returns>
    /// <exception cref="DivideByZeroException">If <paramref name="dpi" /> was 0.</exception>
    [UsedImplicitly]
    public static double ToInches(this double pixels, double dpi = 96D)
    {
        if (dpi == 0)
            throw new DivideByZeroException($"{nameof(dpi)} cannot be 0.");

        return Math.Abs(pixels / dpi);
    }

    /// <summary>
    ///     Get an absolute value in pixels represented by the specified number of inches.
    /// </summary>
    /// <param name="inches">The number of inches to convert.</param>
    /// <param name="dpi">The DPI (dots per inch) of the screen; default is <c>96</c>.</param>
    /// <returns>The pixels equivalent of the inches.</returns>
    [UsedImplicitly]
    public static double ToPixels(this double inches, double dpi = 96D) => Math.Abs(inches * dpi);

    /// <summary>
    ///     Formats a size in bytes (represented by an <see cref="ulong" /> value) to the next closest base-2 size
    ///     representation and appends its suffix to the end; uses <c>1024</c> as the conversion factor.
    /// </summary>
    /// <param name="sizeInBytes">The size, in bytes, to format.</param>
    /// <returns>The number formatted with its suffix as a <see cref="string" />.</returns>
    public static string FormatSize(this ulong sizeInBytes)
    {
        var sizes = Enum.GetValues<MemorySize>();
        var counter = 0;
        decimal size = sizeInBytes;

        while (Math.Round(size / 1024) >= 1)
        {
            size /= 1024;
            counter++;
        }

        return $"{size:F} " +
               $"{sizes[counter].ToDescriptionString()}" +
               $"{(size > 1 && sizes[counter].Equals(MemorySize.Byte) ? "s" : string.Empty)}";
    }
}