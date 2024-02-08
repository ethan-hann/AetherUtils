using System.Drawing.Imaging;

namespace AetherUtils.Core.Extensions;

/// <summary>
/// Provides extension methods for manipulating <see cref="Stream"/> objects.
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// Get an <see cref="Image"/> represented by a <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> containing properly formatted image data.</param>
    /// <returns>A new <see cref="Image"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="stream"/> was <c>null</c>.</exception>
    /// <exception cref="FormatException">If <paramref name="stream"/> did not have a valid <see cref="ImageFormat"/>.</exception>
    public static Image ToImage(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));
       
        try { return Image.FromStream(stream); }
        catch (OutOfMemoryException ex) { throw new FormatException("The image format was not recognized.", ex); }
        catch (ArgumentException ex) { throw new FormatException("The image format was not recognized.", ex); }
    }
}