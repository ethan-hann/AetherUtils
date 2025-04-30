// ImageComboBoxItem.cs : AetherUtils
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

using AetherUtils.Core.WinForms.Controls;
using JetBrains.Annotations;

namespace AetherUtils.Core.WinForms.Models;

/// <summary>
///     Represents an item for an <see cref="ImageComboBox{T}" />.
/// </summary>
/// <param name="text">The text string to display in the combo box.</param>
/// <param name="icon">The image to display to the left of the <paramref name="text" />.</param>
[UsedImplicitly]
public class ImageComboBoxItem(string text, Image icon)
{
    /// <summary>
    ///     The string to display in the combo box.
    /// </summary>
    public string Text { get; [UsedImplicitly] set; } = text;

    /// <summary>
    ///     An image to display to the left of <see cref="Text" />.
    /// </summary>
    public Image Icon { get; [UsedImplicitly] set; } = icon;

    /// <summary>
    ///     Returns just the <see cref="Text" /> of this item; no image.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Text;
}