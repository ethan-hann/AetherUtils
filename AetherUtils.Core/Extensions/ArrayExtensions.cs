using System.Diagnostics;
using System.Drawing.Imaging;

namespace AetherUtils.Core.Extensions;

/// <summary>
/// Provides extension methods for manipulating <see cref="Array"/> objects.
/// </summary>
public static class ArrayExtensions
{
    /// <summary>
    /// Get an <see cref="Image"/> represented by the <see cref="byte"/> array.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/> array containing properly formatted image bytes.</param>
    /// <returns>A new <see cref="Image"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="bytes"/> was <c>null</c>.</exception>
    /// <exception cref="FormatException">If <paramref name="bytes"/> did not have a valid <see cref="ImageFormat"/>.</exception>
    public static Image ToImage(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        
        try
        {
            using var ms = new MemoryStream(bytes);
            return Image.FromStream(ms);
        }
        catch (OutOfMemoryException ex) { throw new FormatException("The image format was not recognized.", ex); }
        catch (ArgumentException ex) { throw new FormatException("The image format was not recognized.", ex); }
    }
}