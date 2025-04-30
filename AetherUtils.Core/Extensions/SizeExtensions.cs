// // SizeExtensions.cs : AetherUtils
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

namespace AetherUtils.Core.Extensions;

/// <summary>
///     Provides extension methods for manipulating <see cref="Size" /> objects.
/// </summary>
[UsedImplicitly]
public static class SizeExtensions
{
    /// <summary>
    ///     Get the center <see cref="Point" /> for the specified <see cref="Size" />.
    /// </summary>
    /// <param name="size">The <see cref="Size" /> to get the center of.</param>
    /// <returns>A <see cref="Point" /> representing the center point of the <see cref="Size" />.</returns>
    [UsedImplicitly]
    public static Point GetCenterPoint(this Size size) => new(size.Width / 2, size.Height / 2);

    /// <summary>
    ///     Get the center <see cref="PointF" /> for the specified <see cref="SizeF" />.
    /// </summary>
    /// <param name="size">The <see cref="SizeF" /> to get the center of.</param>
    /// <returns>A <see cref="PointF" /> representing the center point of the <see cref="SizeF" />.</returns>
    [UsedImplicitly]
    public static PointF GetCenterPointF(this SizeF size) => new(size.Width / 2.0f, size.Height / 2.0f);
}