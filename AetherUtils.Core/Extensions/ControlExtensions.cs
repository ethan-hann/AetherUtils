// // ControlExtensions.cs : AetherUtils
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

using System.ComponentModel;
using JetBrains.Annotations;

namespace AetherUtils.Core.Extensions;

/// <summary>
///     Provides extension methods for manipulating WinForm <see cref="Control" /> objects.
/// </summary>
public static class ControlExtensions
{
    /// <summary>
    ///     Invoke the specified action on the specified control, if required.
    /// </summary>
    /// <typeparam name="T">The <see cref="Control" /> type.</typeparam>
    /// <param name="control">The <see cref="System.Windows.Forms.Control" /> to invoke an action on.</param>
    /// <param name="action">The <see cref="System.Action" /> to invoke on the control.</param>
    [UsedImplicitly]
    public static void InvokeIfRequired<T>(this T control, Action<T> action) where T : ISynchronizeInvoke
    {
        if (control.InvokeRequired)
            control.Invoke(new Action(() => action(control)), null);
        else
            action(control);
    }

    /// <summary>
    ///     Resizes the columns of a <see cref="ListView" /> control to be a best-fit compromise between the header and the
    ///     content.
    /// </summary>
    /// <param name="listView">The <see cref="ListView" /> to auto-size the columns of.</param>
    [UsedImplicitly]
    public static void ResizeColumns(this ListView listView)
    {
        //Prevents flickering
        listView.BeginUpdate();

        //Auto size using header
        listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

        //Grab column size based on header into a dictionary {index, width}
        var columnSize = listView.Columns.Cast<ColumnHeader>()
            .ToDictionary(colHeader => colHeader.Index, colHeader => colHeader.Width);

        //Auto size using data first
        listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

        //Grab column size based on data and set max width
        foreach (ColumnHeader colHeader in listView.Columns)
            colHeader.Width = Math.Max(columnSize.GetValueOrDefault(colHeader.Index, 50),
                colHeader.Width);

        listView.EndUpdate();
    }

    /// <summary>
    ///     Apply a font to this control. This method is thread-safe.
    /// </summary>
    /// <param name="c">The <see cref="Control" /> to apply the font to.</param>
    /// <param name="font">The <see cref="Font" /> to apply to the control.</param>
    public static void ApplyFont(this Control c, Font font)
    {
        if (c.InvokeRequired)
            c.Invoke(() => { c.Font = font; });
        else
            c.Font = font;
    }
}