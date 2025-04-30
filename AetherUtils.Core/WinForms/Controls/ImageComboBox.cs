// // ImageComboBox.cs : AetherUtils
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

using AetherUtils.Core.WinForms.Models;
using JetBrains.Annotations;

namespace AetherUtils.Core.WinForms.Controls;

/// <summary>
///     A special <see cref="ComboBox" /> that allows for images to be displayed next to the text string in the combobox.
///     <typeparam name="T">This combo box only accepts items of type <see cref="ImageComboBoxItem" />.</typeparam>
/// </summary>
[UsedImplicitly]
public class ImageComboBox<T> : ComboBox where T : ImageComboBoxItem
{
    /// <summary>
    ///     Create a new <see cref="ImageComboBox{T}" />.
    /// </summary>
    public ImageComboBox()
    {
        DrawMode = DrawMode.OwnerDrawFixed;
    }

    /// <summary>
    ///     Create a new <see cref="ImageComboBox{T}" /> with the specified starting item.
    /// </summary>
    /// <param name="item"></param>
    public ImageComboBox(ImageComboBoxItem item) : this()
    {
        Items.Add(item);
    }

    /// <summary>
    ///     Create a new <see cref="ImageComboBox{T}" /> with the specified items.
    /// </summary>
    /// <param name="items"></param>
    public ImageComboBox(T[] items) : this()
    {
        foreach (var item in items)
            Items.Add(item);
    }

    /// <inheritdoc />
    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (e.Index < 0) return;

        e.DrawBackground();
        e.DrawFocusRectangle();

        if (Items[e.Index] is ImageComboBoxItem item)
        {
            var bounds = e.Bounds;
            var flagSize = new Size(16, 16);

            e.Graphics.DrawImage(item.Icon, bounds.Left, bounds.Top, flagSize.Width, flagSize.Height);

            using var brush = new SolidBrush(e.ForeColor);

            if (e.Font is { } font)
                e.Graphics.DrawString(item.Text, font, brush, bounds.Left + flagSize.Width + 5, bounds.Top + (bounds.Height - font.Height) / 2.0f);
        }
        else
            base.OnDrawItem(e);
    }
}