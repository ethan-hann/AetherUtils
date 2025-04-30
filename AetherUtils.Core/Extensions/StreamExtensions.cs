// StreamExtensions.cs : AetherUtils
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

using System.Drawing.Imaging;
using JetBrains.Annotations;

namespace AetherUtils.Core.Extensions;

/// <summary>
///     Provides extension methods for manipulating <see cref="Stream" /> objects.
/// </summary>
[UsedImplicitly]
public static class StreamExtensions
{
    /// <summary>
    ///     Get an <see cref="Image" /> represented by a <see cref="Stream" />.
    /// </summary>
    /// <param name="stream">The <see cref="Stream" /> containing properly formatted image data.</param>
    /// <returns>A new <see cref="Image" />.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="stream" /> was <c>null</c>.</exception>
    /// <exception cref="FormatException">If <paramref name="stream" /> did not have a valid <see cref="ImageFormat" />.</exception>
    [UsedImplicitly]
    public static Image ToImage(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        try { return Image.FromStream(stream); }
        catch (OutOfMemoryException ex) { throw new FormatException("The image format was not recognized.", ex); }
        catch (ArgumentException ex) { throw new FormatException("The image format was not recognized.", ex); }
    }
}