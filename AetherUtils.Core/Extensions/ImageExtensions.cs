using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using AetherUtils.Core.Files;

namespace AetherUtils.Core.Extensions;

/// <summary>
/// Provides extension methods for manipulating <see cref="Image"/> objects.
/// </summary>
public static class ImageExtensions
{
    /// <summary>
    /// Resize the <see cref="Image"/> to the specified width and height.
    /// </summary>
    /// <param name="image">The <see cref="Image"/> to resize.</param>
    /// <param name="newWidth">The width to resize to.</param>
    /// <param name="newHeight">The height to resize to.</param>
    /// <returns>The resized <see cref="Image"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="image"/> was <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="newWidth"/> or
    /// <paramref name="newHeight"/> was less than 0.</exception>
    public static Image ResizeImage(this Image image, int newWidth, int newHeight)
    {
        ArgumentNullException.ThrowIfNull(image, nameof(image));
        if (newWidth < 0)
            throw new InvalidOperationException("New width must be greater than 0.");
        if (newHeight < 0)
            throw new InvalidOperationException("New height must be greater than 0");
        
        var destRect = new Rectangle(0, 0, newWidth, newHeight);
        var destImage = new Bitmap(newWidth, newHeight);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
        using var graphics = Graphics.FromImage(destImage);
        graphics.CompositingMode = CompositingMode.SourceCopy;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        using var wrapMode = new ImageAttributes();
        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

        return destImage;
    }
    
    /// <summary>
    /// Resize the image to be the desired width and height.
    /// <para>This method will attempt to maintain the aspect ratio.</para>
    /// </summary>
    /// <param name="image">The <see cref="Image"/> to resize.</param>
    /// <param name="desiredWidth">The desired width of the new image.</param>
    /// <param name="desiredHeight">The desired height of the new image.</param>
    /// <returns>The scaled <see cref="Image"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="image"/> was <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="desiredWidth"/> or
    /// <paramref name="desiredHeight"/> was less than 0.</exception>
    public static Image ResizeImageScaled(this Image image, float desiredWidth, float desiredHeight)
    {
        ArgumentNullException.ThrowIfNull(image, nameof(image));
        if (desiredWidth < float.Epsilon)
            throw new InvalidOperationException("New width must be greater than 0");
        if (desiredHeight < float.Epsilon)
            throw new InvalidOperationException("New height must be greater than 0");
        
        var scaleHeight = desiredHeight / image.Height;
        var scaleWidth = desiredWidth / image.Width;
        var scale = Math.Min(scaleHeight, scaleWidth);
        return new Bitmap(image, (int)(image.Width * scale), (int)(image.Height * scale));
    }
    
    /// <summary>
    /// Convert an <see cref="Image"/> into a <see cref="byte"/> array.
    /// </summary>
    /// <param name="image">The image to convert.</param>
    /// <returns>A <see cref="byte"/> array representing the image.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="image"/> was <c>null</c>.</exception>
    public static byte[] ToByteArray(this Image image)
    {
        ArgumentNullException.ThrowIfNull(image, nameof(image));
        
        using var ms = new MemoryStream();
        image.Save(ms, image.RawFormat);
        return ms.ToArray();
    }

    /// <summary>
    /// Get a value indicating if the image specified by the <paramref name="filePath"/> is a valid image file.
    /// </summary>
    /// <param name="filePath">The file path to an image file.</param>
    /// <returns><c>true</c> if the image was valid; <c>false</c> otherwise.</returns>
    /// <exception cref="FormatException">If the image specified by <paramref name="filePath"/> was not in the correct format.</exception>
    /// <exception cref="FileNotFoundException">If the file specified by <paramref name="filePath"/> was not found.</exception>
    public static bool IsValidImage(this string filePath)
    {
        filePath = FileHelper.ExpandPath(filePath);
        if (!FileHelper.DoesFileExist(filePath))
            throw new FileNotFoundException("The image file was not found.", filePath);
        
        try
        {
            using var img = Image.FromFile(filePath);
            return true;
        }
        catch (OutOfMemoryException ex) { throw new FormatException("The image format was not recognized.", ex); }
        catch (FileNotFoundException ex) { throw new FileNotFoundException("The image file was not found.", 
            filePath, ex); }
    }
}