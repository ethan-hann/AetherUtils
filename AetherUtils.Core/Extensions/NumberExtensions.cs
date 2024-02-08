﻿namespace AetherUtils.Core.Extensions;

/// <summary>
/// Provides extension methods for manipulating various number objects.
/// </summary>
public static class NumberExtensions
{
    /// <summary>
    /// Get an absolute value in inches represented by the specified number of pixels.
    /// </summary>
    /// <param name="pixels">The number of pixels to convert.</param>
    /// <param name="dpi">The DPI (dots per inch) of the screen; defaults to <c>96</c>.</param>
    /// <returns>The inches equivalent of the pixels.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="pixels"/> was <c>null</c>.</exception>
    /// <exception cref="DivideByZeroException">If <paramref name="dpi"/> was 0.</exception>
    public static double ToInches(this double pixels, double dpi = 96)
    {
        ArgumentNullException.ThrowIfNull(pixels, nameof(pixels));
        if (dpi == 0)
            throw new DivideByZeroException($"{nameof(dpi)} cannot be 0.");
        
        return Math.Abs(pixels / dpi);
    }

    /// <summary>
    /// Get an absolute value in pixels represented by the specified number of inches.
    /// </summary>
    /// <param name="inches">The number of inches to convert.</param>
    /// <param name="dpi">The DPI (dots per inch) of the screen; default is <c>96</c>.</param>
    /// <returns>The pixels equivalent of the inches.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="inches"/> was <c>null</c>.</exception>
    public static double ToPixels(this double inches, double dpi = 96)
    {
        ArgumentNullException.ThrowIfNull(inches, nameof(inches));
        return Math.Abs(inches * dpi);
    }

    //TODO: Clean this up and make it more robust!
    /// <summary>
    /// Formats a size in bytes (represented by a <see cref="long"/> value) to the next closest base-2 size
    /// representation and appends its suffix to the end; uses <c>1024</c> as the conversion factor.
    /// </summary>
    /// <param name="sizeInBytes">The size, in bytes, to format.</param>
    /// <param name="conversionFactor">How many bytes are in 1KB? (Default is <c>1024</c>)</param>
    /// <returns>The number formatted with its suffix as a <see cref="string"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="sizeInBytes"/> was <c>null</c>.</exception>
    public static string FormatSize(this long sizeInBytes, int conversionFactor = 1024)
    {
        ArgumentNullException.ThrowIfNull(sizeInBytes, nameof(sizeInBytes));
        
        string[] suffixes = ["Bit", "Nibble", "Byte", "KB", "MB", "GB", "TB", "PB"];
        var counter = 0;
        decimal number = sizeInBytes;

        if (conversionFactor <= 0) { return string.Empty; } //divide-by-zero check.

        while (Math.Round(number / conversionFactor) >= 1)
        {
            number /= conversionFactor;
            counter++;
        }

        return $"{number:n2} {suffixes[counter]}";
    }
}