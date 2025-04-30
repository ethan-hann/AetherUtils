// // SplitButton.cs : AetherUtils
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
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace AetherUtils.Core.WinForms.Controls;

/// <summary>
///     Provides a button containing a split-arrow that can be assigned a context menu.
/// </summary>
[UsedImplicitly]
public partial class SplitButton : Button
{
    /// <summary>
    ///     Create a new split button with a default <see cref="SplitWidth" /> of 20 pixels.
    /// </summary>
    public SplitButton()
    {
        SplitWidth = 20;
    }

    /// <summary>
    ///     The <see cref="ContextMenuStrip" /> to assign to this split button.
    /// </summary>
    [DefaultValue(null)] [Browsable(true)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ContextMenuStrip? Menu { get; [UsedImplicitly] set; }

    /// <summary>
    ///     The width (in pixels) between the right-edge of the button and the splitter graphic.
    /// </summary>
    [DefaultValue(20)] [Browsable(true)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [UsedImplicitly]
    public int SplitWidth { get; [UsedImplicitly] set; }

    /// <inheritdoc />
    protected override void OnMouseDown(MouseEventArgs mevent)
    {
        var splitRect = new Rectangle(Width - SplitWidth, 0, SplitWidth, Height);

        // Figure out if the button click was on the button itself or the menu split
        if (Menu != null && mevent.Button == MouseButtons.Left && splitRect.Contains(mevent.Location))
        {
            Menu.Show(this, 0, Height); // Shows menu under button
        }
        else
        {
            base.OnMouseDown(mevent);
        }
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);

        if (Menu == null || SplitWidth <= 0)
            return;
        // Draw the arrow glyph on the right side of the button
        var arrowX = ClientRectangle.Width - 14;
        var arrowY = ClientRectangle.Height / 2 - 1;

        var arrowBrush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
        var arrows = new[]
        {
            new Point(arrowX, arrowY), new Point(arrowX + 7, arrowY), new Point(arrowX + 3, arrowY + 4)
        };
        pevent.Graphics.FillPolygon(arrowBrush, arrows);

        // Draw a dashed separator on the left of the arrow
        var lineX = ClientRectangle.Width - SplitWidth;
        var lineYFrom = arrowY - 4;
        var lineYTo = arrowY + 8;
        using var separatorPen = new Pen(Brushes.DarkGray);
        separatorPen.DashStyle = DashStyle.Dot;
        pevent.Graphics.DrawLine(separatorPen, lineX, lineYFrom, lineX, lineYTo);
    }
}