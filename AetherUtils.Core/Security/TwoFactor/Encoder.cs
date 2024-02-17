using System.Text;

namespace AetherUtils.Core.Security.TwoFactor;

/// <summary>
/// Class that handles encoding strings and byte arrays to/from URLs and Base-32.
/// </summary>
public static class Encoder
{
    /// <summary>
    /// The alpha-numeric and special characters allowed in a URL.
    /// </summary>
    private const string BIF_URL_ENCODE_ALPHABET = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
    
    /// <summary>
    /// Encode the string <paramref name="value"/> to a valid URL string according to the OATH specification.
    /// </summary>
    /// <param name="value">The string to URL encode.</param>
    /// <returns>A URL encoded string.</returns>
    public static string UrlEncode(string value)
    {
        var builder = new StringBuilder();

        foreach (var symbol in value)
        {
            if (BIF_URL_ENCODE_ALPHABET.Contains(symbol))
                builder.Append(symbol);
            else
            {
                builder.Append('%');
                builder.Append(((int)symbol).ToString("X2"));
            }
        }

        return builder.ToString();
    }

    /// <summary>
    /// Encode the <see cref="byte"/> array of data to a base-32 encoded string.
    /// </summary>
    /// <param name="data">The <see cref="byte"/> array containing the data.</param>
    /// <returns>A string representing the <paramref name="data"/> in base-32.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="data"/> is <c>null</c> or empty.</exception>
    public static string Base32Encode(byte[] data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        if (data.Length == 0) throw new ArgumentNullException(nameof(data));

        var charCount = (int) Math.Ceiling(data.Length / 5d) * 8;
        var returnArray = new char[charCount];

        byte nextChar = 0, bitsRemaining = 5;
        var arrayIndex = 0;

        foreach (var b in data)
        {
            nextChar = (byte) (nextChar | (b >> (8 - bitsRemaining)));
            returnArray[arrayIndex++] = ValueToChar(nextChar);

            if (bitsRemaining < 4)
            {
                nextChar = (byte) ((b >> (3 - bitsRemaining)) & 31);
                returnArray[arrayIndex++] = ValueToChar(nextChar);
                bitsRemaining += 5;
            }

            bitsRemaining -= 3;
            nextChar = (byte) ((b << bitsRemaining) & 31);
        }
        
        if (arrayIndex == charCount) return new string(returnArray);
        
        //If we didn't end with a full character, add padding.
        returnArray[arrayIndex++] = ValueToChar(nextChar);
        while (arrayIndex != charCount) returnArray[arrayIndex++] = '='; //padding

        return new string(returnArray);
    }
    
    /// <summary>
    /// Encode the base-32 <see cref="string"/> to a <see cref="byte"/> array.
    /// </summary>
    /// <param name="input">The base-32 encoded string.</param>
    /// <returns>A <see cref="byte"/> array.</returns>
    /// <exception cref="ArgumentException">If <paramref name="input"/> is <c>null</c> or empty.</exception>
    public static byte[] ToBytes(string input)
    {
        ArgumentException.ThrowIfNullOrEmpty(input, nameof(input));

        input = input.TrimEnd('='); //remove padding characters
        var byteCount = input.Length * 5 / 8; //this must be TRUNCATED
        var returnArray = new byte[byteCount];

        byte curByte = 0, bitsRemaining = 8;
        var arrayIndex = 0;

        foreach (var cValue in input.Select(CharToValue))
        {
            int mask;
            if (bitsRemaining > 5)
            {
                mask = cValue << (bitsRemaining - 5);
                curByte = (byte) (curByte | mask);
                bitsRemaining -= 5;
            }
            else
            {
                mask = cValue >> (5 - bitsRemaining);
                curByte = (byte) (curByte | mask);
                returnArray[arrayIndex++] = curByte;
                curByte = (byte) (cValue << (3 + bitsRemaining));
                bitsRemaining += 3;
            }
        }

        //if we didn't end with a full byte
        if (arrayIndex != byteCount)
            returnArray[arrayIndex] = curByte;

        return returnArray;
    }
    
    /// <summary>
    /// Convert a <see cref="char"/> to its <see cref="int"/> equivalent.
    /// </summary>
    /// <param name="c">The <see cref="char"/> to convert.</param>
    /// <returns>An <see cref="int"/> representing the <see cref="char"/>.</returns>
    /// <exception cref="ArgumentException">If <paramref name="c"/> was not a valid BASE32 character.</exception>
    private static int CharToValue(char c)
    {
        return (int)c switch
        {
            < 91 and > 64 => c - 65, //65-90 == uppercase letters
            < 56 and > 49 => c - 24, //50-55 == numbers 2-7
            < 123 and > 96 => c - 97, //97-122 == lowercase letters
            _ => throw new ArgumentException("Character is not a Base32 character.", nameof(c))
        };
    }
    
    /// <summary>
    /// Convert a <see cref="byte"/> value to its <see cref="char"/> equivalent.
    /// </summary>
    /// <param name="b">The <see cref="byte"/> to convert.</param>
    /// <returns>A <see cref="char"/> representing the <see cref="byte"/>.</returns>
    /// <exception cref="ArgumentException">If <paramref name="b"/> was not a valid BASE32 value.</exception>
    private static char ValueToChar(byte b)
    {
        return b switch
        {
            < 26 => (char)(b + 65),
            < 32 => (char)(b + 24),
            _ => throw new ArgumentException("Byte is not a valid Base32 value.", nameof(b))
        };
    }
}