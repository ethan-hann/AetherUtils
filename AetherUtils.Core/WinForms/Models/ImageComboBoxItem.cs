using AetherUtils.Core.WinForms.Controls;

namespace AetherUtils.Core.WinForms.Models;

/// <summary>
/// Represents an item for an <see cref="ImageComboBox{T}"/>.
/// </summary>
/// <param name="text">The text string to display in the combo box.</param>
/// <param name="icon">The image to display to the left of the <paramref name="text"/>.</param>
public class ImageComboBoxItem(string text, Image icon)
{
    /// <summary>
    /// The string to display in the combo box.
    /// </summary>
    public string Text { get; set; } = text;

    /// <summary>
    /// An image to display to the left of <see cref="Text"/>.
    /// </summary>
    public Image Icon { get; set; } = icon;

    /// <summary>
    /// Returns just the <see cref="Text"/> of this item; no image.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Text;
    }
}