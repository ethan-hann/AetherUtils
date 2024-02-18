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

    /// <summary>
    /// Convert a hexadecimal string representation of bytes into its equivalent <see cref="byte"/> array.
    /// </summary>
    /// <param name="byteString">The string containing the hexadecimal values.
    /// This string can contain values seperated by '-' (<c>CE-B7-8D</c>) or not (<c>CEB78D</c>).</param>
    /// <returns>A <see cref="byte"/> array representing the hexadecimal string.</returns>
    public static IEnumerable<byte> FromPrintableString(this string byteString)
    {
        var hex = byteString.Trim();
        if (hex.Contains('-'))
            hex = hex.Replace("-", "");
        
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }
    

    /// <summary>
    /// Convert an array of values to a string using the specified prefix, delimiter, and optional suffix.
    /// </summary>
    /// <example>
    /// <code>
    /// string[] values = ["val1", "val2", "val3"];
    /// string str = values.ToStringList(',', "PRE(", ")");
    /// returns: "PRE(val1,val2,val3)"
    /// </code>
    /// </example>
    /// <param name="values">The array of values to combine.</param>
    /// <param name="delimiter">The delimiter to separate the values with; default is <c>','</c></param>
    /// <param name="prefix">The prefix to add to the resulting string; default is an empty string.</param>
    /// <param name="suffix">The suffix to add to the resulting string; default is an empty string.</param>
    /// <typeparam name="T">The type of objects contained within the array.</typeparam>
    /// <returns>A string formatted with the <paramref name="values"/> seperated by the <paramref name="delimiter"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="values"/>, <paramref name="prefix"/>,
    /// or <paramref name="delimiter"/> was <c>null</c>.</exception>
    public static string ToStringList<T>(this T[] values, char delimiter = ',', string prefix = "", string suffix = "")
    {
        ArgumentNullException.ThrowIfNull(values, nameof(values));
        ArgumentNullException.ThrowIfNull(prefix, nameof(prefix));
        ArgumentNullException.ThrowIfNull(delimiter, nameof(delimiter));
        
        StringBuilder sb = new(prefix);
        for (var i = 0; i < values.Length - 1; i++)
            sb = sb.Append($"{values[i]?.ToString()}{delimiter}");
        sb = sb.Append(suffix);
        return sb.ToString();
    }
}