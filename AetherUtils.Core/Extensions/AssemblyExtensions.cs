// // AssemblyExtensions.cs : AetherUtils
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

using System.Drawing.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace AetherUtils.Core.Extensions;

/// <summary>
///     Provides extension methods for getting data from an <see cref="Assembly" />.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    ///     Get the specified embedded resource from the <see cref="Assembly" />.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly" /> to read the resource from.</param>
    /// <param name="resourceName">The name of the resource, including the namespace.</param>
    /// <returns>A string representing the resource, or <c>null</c> if the resource wasn't found.</returns>
    [UsedImplicitly]
    public static string? GetStringResource(this Assembly assembly, string resourceName)
    {
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) { return null; }

        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }

    /// <summary>
    ///     Get the specified embedded resource from the <see cref="Assembly" /> as a raw stream.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly" /> to read the resource from.</param>
    /// <param name="resourceName">The name of the resource, including the namespace.</param>
    /// <returns>
    ///     A raw <see cref="Stream" /> representing the resource,
    ///     or <c>null</c> if the resource wasn't found.
    /// </returns>
    [UsedImplicitly]
    public static Stream? GetResourceAsStream(this Assembly assembly, string resourceName) =>
        assembly.GetManifestResourceStream(resourceName);


    /// <summary>
    ///     Get the specified embedded font resource from the <see cref="Assembly" /> and add it
    ///     to the <see cref="PrivateFontCollection" />.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly" /> to read the resource from.</param>
    /// <param name="resourceName">The name of the resource, including the namespace.</param>
    /// <param name="fontCollection">The <see cref="PrivateFontCollection" /> to add the read font to.</param>
    /// <returns>
    ///     The modified <see cref="PrivateFontCollection" /> after the font has been added, or the original
    ///     <see cref="PrivateFontCollection" /> if the font could not be added.
    /// </returns>
    public static PrivateFontCollection AddFontResource(this Assembly assembly, string resourceName,
        PrivateFontCollection fontCollection)
    {
        ArgumentException.ThrowIfNullOrEmpty(resourceName, nameof(resourceName));
        ArgumentNullException.ThrowIfNull(fontCollection, nameof(fontCollection));

        var stream = assembly.GetResourceAsStream(resourceName);
        if (stream == null) return fontCollection;

        //Read the font data from the stream into a new array of bytes
        var fontData = new byte[stream.Length];
        _ = stream.Read(fontData, 0, (int)stream.Length);
        stream.Close();

        // Allocate memory for the font data and copy it
        var fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
        Marshal.Copy(fontData, 0, fontPtr, fontData.Length);

        fontCollection.AddMemoryFont(fontPtr, fontData.Length);

        // Free the allocated memory
        Marshal.FreeCoTaskMem(fontPtr);

        return fontCollection;
    }
}