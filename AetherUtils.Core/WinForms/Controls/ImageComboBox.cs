using AetherUtils.Core.WinForms.Models;

namespace AetherUtils.Core.WinForms.Controls;

/// <summary>
/// A special <see cref="ComboBox"/> that allows for images to be displayed next to the text string in the combobox.
/// <typeparam name="T">This combo box only accepts items of type <see cref="ImageComboBoxItem"/>.</typeparam>
/// </summary>
public class ImageComboBox<T> : ComboBox where T : ImageComboBoxItem
{
    /// <summary>
    /// Create a new <see cref="ImageComboBox{T}"/>.
    /// </summary>
    public ImageComboBox()
    {
        DrawMode = DrawMode.OwnerDrawFixed;
    }

    /// <summary>
    /// Create a new <see cref="ImageComboBox{T}"/> with the specified starting item.
    /// </summary>
    /// <param name="item"></param>
    public ImageComboBox(ImageComboBoxItem item) : this() => Items.Add(item);

    /// <summary>
    /// Create a new <see cref="ImageComboBox{T}"/> with the specified items.
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
            e.Graphics.DrawString(item.Text, e.Font, brush, bounds.Left + flagSize.Width + 5, bounds.Top + (bounds.Height - e.Font.Height) / 2);
        }
        else
            base.OnDrawItem(e);
    }
}