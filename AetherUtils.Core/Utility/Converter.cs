using AetherUtils.Core.Extensions;
using AetherUtils.Core.RegEx;
using AetherUtils.Core.Security;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Utility
{
    /// <summary>
    /// Extension class that provides functions to encode/decode strings and byte arrays to/from different <see cref="HashEncoding"/>s.
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Encode the string to the specified <see cref="HashEncoding"/>.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="encoding"></param>
        /// <returns>A string that has been encoded using the specified <see cref="HashEncoding"/>.</returns>
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
        /// Encode the string to the specified <see cref="HashEncoding"/>.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="encoding"></param>
        /// <returns>A string that has been encoded using the specified <see cref="HashEncoding"/>.</returns>
        public static string BytesToEncodedString(this byte[] bytes, HashEncoding encoding)
        {
            string byteString = Encoding.UTF8.GetString(bytes);
            return StringToEncodedString(byteString, encoding);
        }

        /// <summary>
        /// Encode the string to the specified <see cref="HashEncoding"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns>An array of encoded <see cref="byte"/>s.</returns>
        public static byte[] StringToEncodedBytes(this string value, HashEncoding encoding)
        {
            string encodedString = value.StringToEncodedString(encoding); //Encode input string
            return Encoding.UTF8.GetBytes(encodedString); //Get bytes in UTF8
        }

        /// <summary>
        /// Encode the <see cref="byte"/> array to the specifed <see cref="HashEncoding"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns>An array of encoded <see cref="byte"/>s.</returns>
        public static byte[] BytesToEncodedBytes(this byte[] value, HashEncoding encoding)
        {
            string byteString = Encoding.UTF8.GetString(value);
            return byteString.StringToEncodedBytes(encoding);
        }

        /// <summary>
        /// Decodes the encoded string to according to the specified <see cref="HashEncoding"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns>A decoded string.</returns>
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
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns>A <see cref="byte"/> array of decoded bytes.</returns>
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
        /// Get a <see cref="byte"/> array representing the string using <see cref="Encoding.UTF8"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] BytesFromString(this string value) => Encoding.UTF8.GetBytes(value);
    }
}
