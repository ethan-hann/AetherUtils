using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AetherUtils.Core.Files;
using AetherUtils.Core.RegEx;
using AetherUtils.Core.Security.Hashing;
using AetherUtils.Core.Structs;

namespace AetherUtils.Core.Extensions;

/// <summary>
/// Provides extension methods for manipulating <see cref="string"/> objects.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Convert the Base64 representation of a picture into a usable <see cref="Image"/> object for drawing.
    /// </summary>
    /// <param name="base64">The <see cref="Image"/> as a Base64 <see cref="string"/>.</param>
    /// <returns>An <see cref="Image"/> or <c>null</c> if <paramref name="base64"/> is an invalid string.</returns>
    /// <exception cref="ArgumentNullException">If <see cref="base64"/> was <c>null</c>.</exception>
    /// <exception cref="FormatException">If the input was not a valid Base64 <see cref="string"/>.</exception>
    public static Image ImageFromString(this string base64)
    {
        ArgumentNullException.ThrowIfNull(base64, nameof(base64));
        
        if (!base64.IsBase64Encoded())
            throw new FormatException("Input string was not Base64 encoded.");
        
        using var ms = new MemoryStream(Convert.FromBase64String(base64));
        return Image.FromStream(ms);
    }
    
    /// <summary>
    /// Convert an unsecure <see cref="string"/> value into a <see cref="SecureString"/>.
    /// </summary>
    /// <param name="unsecure">The unsecure <see cref="string"/>.</param>
    /// <returns>A new read-only <see cref="SecureString"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="unsecure"/> was <c>null</c>.</exception>
    public static SecureString ToSecureString(this string unsecure)
    {
        ArgumentNullException.ThrowIfNull(unsecure, nameof(unsecure));
        
        var secure = new SecureString();
        unsecure.ToCharArray().ToList().ForEach(c => secure.AppendChar(c));
        secure.MakeReadOnly();
        return secure;
    }
    
    //TODO: Find a better way to do this without potentially leaking the secure string into unmanaged memory.
    /// <summary>
    /// Convert a <see cref="SecureString"/> to an unsecured <see cref="string"/>.
    /// </summary>
    /// <param name="secure">The <see cref="SecureString"/>.</param>
    /// <returns>The unsecured <see cref="string"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="secure"/> was <c>null</c>.</exception>
    public static string FromSecureString(this SecureString secure)
    {
        ArgumentNullException.ThrowIfNull(secure, nameof(secure));
        
        var pointer = IntPtr.Zero;
        try
        {
            pointer = Marshal.SecureStringToBSTR(secure);
            return Marshal.PtrToStringBSTR(pointer);
        }
        finally
        {
            Marshal.ZeroFreeBSTR(pointer);
        }
    }
    
    /// <summary>
    /// Trims the <see cref="string"/> to be the length specified before a new-line character is inserted.
    /// <para>If the new-line character would be inserted at the position of a period (<c>.</c>), the
    /// new line is inserted at the index of <c>(.) + 1</c>.
    /// If the line being checked contains a new-line character already, nothing is done for that line.</para>
    /// </summary>
    /// <param name="input">The <see cref="string"/> to trim.</param>
    /// <param name="lineLength">The number of characters in the line; default is <c>80</c>.</param>
    /// <returns>A new <see cref="TrimmedString"/> containing the trimmed <see cref="string"/> and the number of new lines.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="input"/> was <c>null</c>.</exception>
    public static TrimmedString Trim(this string input, int lineLength = 80)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        
        for (var i = 0; i < input.Length; i++)
        {
            if (i % lineLength != 0 || i == 0) continue;
            switch (input[i])
            {
                case '.':
                {
                    if (i + 1 <= input.Length)
                        input = input.Insert(i + 1, "\n");
                    if (i + 2 <= input.Length)
                        input = input.Remove(i + 2, 1);
                    break;
                }
                case '\n':
                    continue;
                default:
                    input = input.Insert(i, "\n");
                    break;
            }
        }
        return new TrimmedString(input);
    }
    
    /// <summary>
    /// Deserializes an object from an XML <see cref="string"/>.
    /// </summary>
    /// <param name="xml">The XML <see cref="string"/> to deserialize.</param>
    /// <typeparam name="T">The type of <see cref="object"/> to deserialize to.</typeparam>
    /// <returns>The deserialized <see cref="object"/> or <c>null</c> if the deserialization failed.</returns>
    /// <exception cref="ArgumentException">If <paramref name="xml"/> was <c>null</c> or empty.</exception>
    public static T? DeserializeXml<T>(this string xml) where T : class
    {
        ArgumentException.ThrowIfNullOrEmpty(xml, nameof(xml));
        
        var serializer = new XmlSerializer(typeof(T));
        using var sr = new StringReader(xml);
        using var reader = XmlReader.Create(sr);
        
        if (serializer.CanDeserialize(reader))
            return (T?)serializer.Deserialize(reader);
        
        return null;
    }

    /// <summary>
    /// Deserializes an object from a JSON <see cref="string"/>.
    /// </summary>
    /// <param name="json">The JSON <see cref="string"/> to deserialize.</param>
    /// <typeparam name="T">The type of <see cref="object"/> to deserialize to.</typeparam>
    /// <returns>The deserialized <see cref="object"/> or <c>null</c> if the deserialization failed.</returns>
    /// <exception cref="ArgumentException">If <paramref name="json"/> was <c>null</c> or empty.</exception>
    public static T? DeserializeJson<T>(this string json) where T : class
    {
        ArgumentException.ThrowIfNullOrEmpty(json, nameof(json));

        var serializer = new Json<T>();
        return serializer.FromJson(json);
    }

    /// <summary>
    /// Get a value indicating if the <see cref="string"/> appears to be Base64 encoded.
    /// </summary>
    /// <param name="input">The <see cref="string"/> to check encoding on.</param>
    /// <returns><c>true</c> if the <see cref="string"/> is Base64 encoded; <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="input"/> was <c>null</c>.</exception>
    public static bool IsBase64Encoded(this string input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        return RegexGenerator.Base64Regex().IsMatch(input);
    }

    /// <summary>
    /// Get a value indicating if the <see cref="string"/> appears to be Hex (Base16) encoded.
    /// </summary>
    /// <param name="input">The <see cref="string"/> to check encoding on.</param>
    /// <returns><c>true</c> if the <see cref="string"/> is Hex encoded; <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="input"/> was <c>null</c>.</exception>
    public static bool IsHexEncoded(this string input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        return RegexGenerator.HexRegex().IsMatch(input);
    }
    
    /// <summary>
    /// Encode the <see cref="string"/> to the specified <see cref="HashEncoding"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to encode.</param>
    /// <param name="encoding">The <see cref="HashEncoding"/> to use.</param>
    /// <returns>A <see cref="string"/> encoded using the specified <see cref="HashEncoding"/>.</returns>
    public static string StringToEncodedString(this string value, HashEncoding encoding)
    {
        return encoding switch
        {
            HashEncoding.Base64 => Convert.ToBase64String(Encoding.UTF8.GetBytes(value)),
            HashEncoding.Hex => Convert.ToHexString(Encoding.UTF8.GetBytes(value)),
            _ => value,
        };
    }

    /// <summary>
    /// Encode the <see cref="byte"/> array to the specified <see cref="HashEncoding"/>.
    /// </summary>
    /// <param name="bytes">A <see cref="byte"/> array to encode.</param>
    /// <param name="encoding">The <see cref="HashEncoding"/> to use.</param>
    /// <returns>A <see cref="string"/> encoded using the specified <see cref="HashEncoding"/>.</returns>
    public static string BytesToEncodedString(this byte[] bytes, HashEncoding encoding)
    {
        string byteString = Encoding.UTF8.GetString(bytes);
        return StringToEncodedString(byteString, encoding);
    }

    /// <summary>
    /// Encode the <see cref="string"/> to the specified <see cref="HashEncoding"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to encode.</param>
    /// <param name="encoding">The <see cref="HashEncoding"/> to use.</param>
    /// <returns>A <see cref="byte"/> array containing the encoded bytes.</returns>
    public static byte[] StringToEncodedBytes(this string value, HashEncoding encoding)
    {
        string encodedString = value.StringToEncodedString(encoding); //Encode input string
        return Encoding.UTF8.GetBytes(encodedString); //Get bytes in UTF8
    }

    /// <summary>
    /// Encode the <see cref="byte"/> array to the specified <see cref="HashEncoding"/>.
    /// </summary>
    /// <param name="bytes">A <see cref="byte"/> array to encode.</param>
    /// <param name="encoding">The <see cref="HashEncoding"/> to use.</param>
    /// <returns>A <see cref="byte"/> array containing the encoded bytes.</returns>
    public static byte[] BytesToEncodedBytes(this byte[] bytes, HashEncoding encoding)
    {
        string byteString = Encoding.UTF8.GetString(bytes);
        return byteString.StringToEncodedBytes(encoding);
    }

    /// <summary>
    /// Decodes an encoded <see cref="string"/> according to the specified <see cref="HashEncoding"/>.
    /// </summary>
    /// <param name="value">The encoded <see cref="string"/>.</param>
    /// <param name="encoding">The <see cref="HashEncoding"/> to use.</param>
    /// <returns>A decoded <see cref="string"/>.</returns>
    public static string DecodedStringFromEncodedString(this string value, HashEncoding encoding)
    {
        return encoding switch
        {
            HashEncoding.Base64 => Encoding.UTF8.GetString(Convert.FromBase64String(value)),
            HashEncoding.Hex => Encoding.UTF8.GetString(Convert.FromHexString(value)),
            _ => value,
        };
    }

    /// <summary>
    /// Decodes the encoded <see cref="string"/> according to the specified <see cref="HashEncoding"/>.
    /// </summary>
    /// <param name="value">The encoded <see cref="string"/>.</param>
    /// <param name="encoding">The <see cref="HashEncoding"/> to use.</param>
    /// <returns>A <see cref="byte"/> array containing the decoded bytes.</returns>
    public static byte[] DecodedBytesFromEncodedString(this string value, HashEncoding encoding)
    {
        return encoding switch
        {
            HashEncoding.Base64 => Convert.FromBase64String(value),
            HashEncoding.Hex => Convert.FromHexString(value),
            _ => Encoding.UTF8.GetBytes(value),
        };
    }

    /// <summary>
    /// Get a <see cref="byte"/> array representing the <see cref="string"/> using <see cref="Encoding.UTF8"/>.
    /// </summary>
    /// <param name="value">A <see cref="string"/> to get the bytes of.</param>
    /// <returns>A <see cref="byte"/> array containing the bytes representing the <see cref="string"/>.</returns>
    public static byte[] BytesFromString(this string value) => Encoding.UTF8.GetBytes(value);
    
    /// <summary>
    /// Removes all whitespace characters from the specified <see cref="string"/>.
    /// </summary>
    /// <param name="str">The string to remove whitespace from.</param>
    /// <returns>A new string, without the whitespace characters.</returns>
    public static string RemoveWhitespace(this string str) => new(str.Where(c => !char.IsWhiteSpace(c)).ToArray());
}