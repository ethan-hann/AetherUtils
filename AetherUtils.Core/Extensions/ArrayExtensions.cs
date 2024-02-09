using System.Diagnostics;
using System.Drawing.Imaging;
using System.Text;

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

    /// <summary>
    /// Convert the numeric value of each element of a specified array of bytes to
    /// its equivalent hexadecimal string representation.
    /// </summary>
    /// <param name="bytes">An <see cref="IEnumerable{T}"/> of bytes.</param>
    /// <returns>A string of hexadecimal pairs separated by hyphens, where each pair represents the corresponding
    /// element in value; for example, "7F-2C-4A-00".</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="bytes"/> was <c>null</c>.</exception>
    public static string ToPrintableString(this IEnumerable<byte> bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        return BitConverter.ToString(bytes.ToArray());
    }
}