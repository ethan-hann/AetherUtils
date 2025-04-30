// // FontHandler.cs : AetherUtils
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
using AetherUtils.Core.Extensions;
using JetBrains.Annotations;

namespace AetherUtils.Core.Reflection;

/// <summary>
///     <para>
///         Handles loading, adding, and applying fonts from embedded resources to the active <see cref="Assembly" />
///         and controls. This class cannot be used for loading fonts from disk. Font files must be included in the active
///         assembly and embedded.
///     </para>
///     <para>
///         Embedded fonts can be added to the internal font collection by using <see cref="AddFont" />. Once added,
///         these fonts can be applied to controls using <see cref="ApplyFont" />.
///     </para>
///     <para>This class cannot be inherited or instantiated.</para>
/// </summary>
[UsedImplicitly]
public sealed class FontHandler
{
    private static readonly object Lock = new();
    private static FontHandler? _instance;

    private readonly Dictionary<string, string> _fontDict = new();

    private PrivateFontCollection _privateFonts = new();

    private FontHandler() { }

    /// <summary>
    ///     Get the single instance of the <see cref="FontHandler" />.
    /// </summary>
    [UsedImplicitly]
    public static FontHandler Instance
    {
        get
        {
            lock (Lock)
            {
                return _instance ??= new FontHandler();
            }
        }
    }

    /// <summary>
    ///     Add the font specified to the font collection.
    /// </summary>
    /// <param name="execAssembly">The currently executing assembly to add the font to.</param>
    /// <param name="fontName">The font family name.</param>
    /// <param name="fontResourcePath">The fully qualified path to the embedded resource.</param>
    [UsedImplicitly]
    public bool AddFont(Assembly execAssembly, string fontName, string fontResourcePath)
    {
        if (_fontDict.ContainsValue(fontResourcePath)) return false;

        _privateFonts = execAssembly.AddFontResource(fontResourcePath, _privateFonts);

        var addedFontName = _privateFonts.Families.LastOrDefault()?.Name;
        return addedFontName != null && _fontDict.TryAdd(fontName, addedFontName);
    }

    /// <summary>
    ///     Apply the specified font family to the control.
    /// </summary>
    /// <param name="control">The <see cref="Control" /> to apply the font to.</param>
    /// <param name="fontName">The name of the font family.</param>
    /// <param name="fontSize">The size of the font to be applied.</param>
    /// <param name="style">The <see cref="FontStyle" /> of the font to be applied.</param>
    /// <returns><c>true</c> if the font was applied; <c>false</c> otherwise.</returns>
    [UsedImplicitly]
    public bool ApplyFont(Control control, string fontName, float fontSize, FontStyle style)
    {
        if (_privateFonts.Families.Length <= 0) return false;

        var fontFamilyName = _fontDict[fontName];
        var customFont = new Font(_privateFonts.Families.First(f => f.Name.Equals(fontFamilyName)),
            fontSize, style);

        control.ApplyFont(customFont);
        return true;
    }
}