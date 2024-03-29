﻿using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace AetherUtils.Core.WinForms.Controls;

public partial class SplitButton : Button
{
    [DefaultValue(null), Browsable(true),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ContextMenuStrip? Menu { get; set; }

    [DefaultValue(20), Browsable(true),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int SplitWidth { get; set; }

    public SplitButton() 
    {
        SplitWidth = 20;
    }

    protected override void OnMouseDown(MouseEventArgs mevent)
    {
        var splitRect = new Rectangle(this.Width - this.SplitWidth, 0, this.SplitWidth, this.Height);

        // Figure out if the button click was on the button itself or the menu split
        if (Menu != null && mevent.Button == MouseButtons.Left && splitRect.Contains(mevent.Location) )
        {
            Menu.Show(this, 0, Height);    // Shows menu under button
            //Menu.Show(this, mevent.Location); // Shows menu at click location
        }
        else
        {
            base.OnMouseDown(mevent);
        }
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);

        if (Menu == null || SplitWidth <= 0)
            return;
        // Draw the arrow glyph on the right side of the button
        var arrowX = ClientRectangle.Width - 14;
        var arrowY = ClientRectangle.Height / 2 - 1;

        var arrowBrush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
        var arrows = new[] { new Point(arrowX, arrowY), new Point(arrowX + 7, arrowY), new Point(arrowX + 3, arrowY + 4) };
        pevent.Graphics.FillPolygon(arrowBrush, arrows);

        // Draw a dashed separator on the left of the arrow
        var lineX = ClientRectangle.Width - this.SplitWidth;
        var lineYFrom = arrowY - 4;
        var lineYTo = arrowY + 8;
        using var separatorPen = new Pen(Brushes.DarkGray);
        separatorPen.DashStyle = DashStyle.Dot;
        pevent.Graphics.DrawLine(separatorPen, lineX, lineYFrom, lineX, lineYTo);
    }
}