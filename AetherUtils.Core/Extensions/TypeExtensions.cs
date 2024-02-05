using AetherUtils.Core.RegEx;
using AetherUtils.Core.Security;
using AetherUtils.Core.Structs;
using Microsoft.CSharp;
using System.CodeDom;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using YamlDotNet.Core.Tokens;

namespace AetherUtils.Core.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Get the friendly, displayable name for the <see cref="Type"/>.
        /// </summary>
        /// <param name="T">The <see cref="Type"/> to get the name of.</param>
        /// <returns>The friendly name for the <see cref="Type"/>.</returns>
        public static string GetFriendlyName(this Type T)
        {
            using var provider = new CSharpCodeProvider();
            var typeRef = new CodeTypeReference(T);
            return provider.GetTypeOutput(typeRef);
        }

        /// <summary>
        /// Get the string associated with a <see cref="DescriptionAttribute"/> attribute on an <see cref="Enum"/> value.
        /// </summary>
        /// <param name="val">The Enum value to get the description string of.</param>
        /// <typeparam name="T">The Enum type to get the attributes of.</typeparam>
        /// <returns></returns>
        public static string ToDescriptionString<T>(this T val) where T : Enum
        {
            if (val.Equals(null)) return string.Empty;
            try
            {
                var type = val.GetType();
                var field = type.GetField(val.ToString());

                if (field == null) return string.Empty;

                if (field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    is not DescriptionAttribute[] attributes) return string.Empty;

                return attributes.Length > 0 ? attributes[0].Description : string.Empty;
            }
            catch (Exception) { return string.Empty; }
        }

        /// <summary>
        /// Convert the BASE64 representation of an image into a usable <see cref="Image"/> object for drawing.
        /// </summary>
        /// <param name="base64">The image as a base64 string.</param>
        /// <returns>An image or <c>null</c> if <paramref name="base64"/> is an invalid string.</returns>
        public static Image? FromBase64String(this string base64)
        {
            try
            {
                using var ms = new MemoryStream(Convert.FromBase64String(base64));
                return Image.FromStream(ms);
            }
            catch (Exception) { return null; }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="newWidth">The width to resize to.</param>
        /// <param name="newHeight">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Image ResizeImage(this Image image, int newWidth, int newHeight)
        {
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
        /// Resize the image to be the desired size. Tries its best to maintain the aspect ratio.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="desiredSize">The size of the new image.</param>
        /// <returns>The resized image.</returns>
        public static Image ResizeImageScaled(this Image image, SizeF desiredSize)
        {
            var scaleHeight = desiredSize.Height / image.Height;
            var scaleWidth = desiredSize.Width / image.Width;
            var scale = Math.Min(scaleHeight, scaleWidth);
            return new Bitmap(image, (int)(image.Width * scale), (int)(image.Height * scale));
        }

        /// <summary>
        /// Resize the image to be the desired size. Tries its best to maintain the aspect ratio.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="desiredWidth">The width of the new image.</param>
        /// <param name="desiredHeight">The height of the new image.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImageScaled(this Image image, float desiredWidth, float desiredHeight)
        {
            return (Bitmap)ResizeImageScaled(image, new SizeF(desiredWidth, desiredHeight));
        }

        /// <summary>
        /// Convert an <see cref="Image"/> into a <see cref="byte"/> array.
        /// </summary>
        /// <param name="image">The image to convert.</param>
        /// <returns>A byte array representing the image.</returns>
        public static byte[] ToByteArray(this Image image)
        {
            try
            {
                using var ms = new MemoryStream();
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
            catch (Exception ex) { Debug.WriteLine(ex); return Array.Empty<byte>(); }
        }

        /// <summary>
        /// Converts a <see cref="byte"/> array into an <see cref="System.Drawing.Image"/>.
        /// </summary>
        /// <param name="bytes">The bytes to convert to an image.</param>
        /// <returns>An image or <c>null</c> if <paramref name="bytes"/> does not
        /// have a valid <see cref="ImageFormat"/>.</returns>
        public static Image? ToImage(this byte[] bytes)
        {
            try
            {
                using var ms = new MemoryStream(bytes);
                return Image.FromStream(ms);
            }
            catch (Exception ex) { Debug.WriteLine(ex); return null; }
        }

        /// <summary>
        /// Get the center <see cref="Point"/> for the specified <see cref="System.Drawing.Size"/>.
        /// </summary>
        /// <param name="size">The <see cref="System.Drawing.Size"/> to get the center of.</param>
        /// <returns>A <see cref="System.Drawing.Point"/> that represents the
        /// center point of the <see cref="System.Drawing.Size"/>.</returns>
        public static Point GetCenterPoint(this Size size) => new(size.Width / 2, size.Height / 2);

        /// <summary>
        /// Get the starting date of the week based on the supplied date.
        /// </summary>
        /// <param name="date">The date to get the start of the week of.</param>
        /// <returns>A <see cref="DateTime"/> representing the start of the week.</returns>
        public static DateTime StartOfWeek(this DateTime date)
        {
            var dt = date;
            while (dt.DayOfWeek != Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                dt = dt.AddDays(-1);
            return dt;
        }

        /// <summary>
        /// Get the starting date of the month based on the supplied date.
        /// </summary>
        /// <param name="date">The date to get the start of the month of.</param>
        /// <returns>A <see cref="DateTime"/> representing the start of the month.</returns>
        public static DateTime StartOfMonth(this DateTime date) => date.AddDays(1 - date.Day);

        /// <summary>
        /// Get the ending date of the month based on the supplied date.
        /// </summary>
        /// <param name="date">The date representing the starting date for the month.</param>
        /// <returns>A <see cref="DateTime"/> representing the end of the month.</returns>
        public static DateTime EndOfMonth(this DateTime date) =>
            new(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

        /// <summary>
        /// Convert an unsecure <see cref="string"/> value into a <see cref="SecureString"/>.
        /// </summary>
        /// <param name="unsecure">The unsecure <see cref="System.String"/>.</param>
        /// <returns>A new <see cref="System.Security.SecureString"/>.</returns>
        public static SecureString ToSecureString(this string unsecure)
        {
            var secure = new SecureString();
            foreach (var character in unsecure.ToCharArray())
                secure.AppendChar(character);
            secure.MakeReadOnly();
            return secure;
        }

        /// <summary>
        /// Convert a <see cref="SecureString"/> to an unsecured string.
        /// </summary>
        /// <param name="secure">The <see cref="SecureString"/>.</param>
        /// <returns>The unsecured <see cref="string"/>.</returns>
        public static string FromSecureString(this SecureString secure)
        {
            var pointer = IntPtr.Zero;
            try
            {
                pointer = Marshal.SecureStringToGlobalAllocUnicode(secure);
                return Marshal.PtrToStringUni(pointer) ?? string.Empty;
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(pointer);
            }
        }

        /// <summary>
        /// Converts a <see cref="Stream"/> into an <see cref="Image"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to convert.</param>
        /// <returns>An <see cref="Image"/> or <c>null</c> if <paramref name="stream"/>
        /// does not have a valid <see cref="ImageFormat"/>.</returns>
        public static Image? ToImage(this Stream stream)
        {
            try
            {
                using (stream)
                    return Image.FromStream(stream);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Formats a size in bytes (represented by a <see cref="long"/> value) to the next closest base-2 size
        /// representation and appends its suffix to the end; uses <c>1024</c> as the conversion factor.
        /// </summary>
        /// <param name="sizeInBytes">The size, in bytes, to format.</param>
        /// <param name="conversionFactor">How many bytes are in 1KB? (Default is <c>1024</c>)</param>
        /// <returns>The number formatted with its suffix as a <see cref="string"/>.</returns>
        public static string FormatSize(this long sizeInBytes, int conversionFactor = 1024)
        {
            string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
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

        /// <summary>
        /// Converts a <see cref="TimeSpan"/> object into a human-readable string in the form of: 
        /// <para><c>w days, x hours, y minutes, z seconds</c></para>
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> to convert.</param>
        /// <returns>A string representing the <see cref="TimeSpan"/> as words.</returns>
        public static string TimeSpanToString(this TimeSpan timeSpan)
        {
            var components = new List<Tuple<int, string>>
            {
                Tuple.Create((int)timeSpan.TotalDays, "day"),
                Tuple.Create(timeSpan.Hours, "hour"),
                Tuple.Create(timeSpan.Minutes, "minute"),
                Tuple.Create(timeSpan.Seconds, "second")
            };

            while (components.Any() && components[0].Item1 == 0)
                components.RemoveAt(0);

            return string.Join(", ",
                components.Select(t => $"{t.Item1} {t.Item2} {(t.Item1 != 1 ? "s" : string.Empty)}"));
        }

        /// <summary>
        /// Convert the specified number of pixels to its inches equivalent.
        /// </summary>
        /// <param name="pixels">The pixels to convert as a <see cref="double"/>.</param>
        /// <param name="dpi">The DPI of the screen used for conversion. (Default is <c>96</c>)</param>
        /// <returns>The inches equivalent of the pixels.</returns>
        public static double ToInches(this double pixels, double dpi = 96) => pixels / dpi;

        /// <summary>
        /// Convert the specified number of inches to its pixels equivalent.
        /// </summary>
        /// <param name="inches">The inches to convert as a <see cref="double"/>.</param>
        /// <param name="dpi">The DPI of the screen used for conversion. (Default is <c>96</c>)</param>
        /// <returns>The pixels equivalent of the inches.</returns>
        public static double ToPixels(this double inches, double dpi = 96) => inches * dpi;

        /// <summary>
        /// Trims the string to be the length specified before a new-line character is inserted.
        /// <para>If the new-line character would be inserted at the position of a period (.), the
        /// new line is inserted at the index of the period + 1.
        /// If the line being checked contains a new-line character already, nothing is done for that line.</para>
        /// </summary>
        /// <param name="T">The string to trim.</param>
        /// <param name="lineLength">The number of characters in the line. Default is 80.</param>
        /// <returns>A new <see cref="TrimmedString"/> containing the trimmed string and the number of new lines.</returns>
        public static TrimmedString Trim(this string T, int lineLength = 80)
        {
            for (var i = 0; i < T.Length; i++)
            {
                if (i % lineLength != 0 || i == 0) continue;
                switch (T[i])
                {
                    case '.':
                        {
                            if (i + 1 <= T.Length)
                                T = T.Insert(i + 1, "\n");
                            if (i + 2 <= T.Length)
                                T = T.Remove(i + 2, 1);
                            break;
                        }
                    case '\n':
                        continue;
                    default:
                        T = T.Insert(i, "\n");
                        break;
                }
            }
            return new TrimmedString(T);
        }

        /// <summary>
        /// Create a CSV file from the <see cref="DataTable"/>.
        /// </summary>
        /// <param name="T">The <see cref="DataTable"/> to create a CSV file from.</param>
        /// <param name="filePath">The absolute path with the file name and extension that the CSV file should be saved to.</param>
        /// <param name="delimiter">The delimiter to separate fields with. Default is a comma (,).</param>
        /// <returns>True if the file was created; False otherwise.</returns>
        /// <exception cref="DirectoryNotFoundException">the containing directories for the <paramref name="filePath"/> were not found.</exception>
        public static bool ToCsvFile(this DataTable T, string filePath, string delimiter = ",")
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                throw new DirectoryNotFoundException($"Destination folder not found: {filePath}");

            try
            {
                var columns = T.Columns.Cast<DataColumn>().ToArray();

                var lines = (new[] { string.Join(delimiter, columns.Select(c => c.ColumnName)) })
                    .Concat(T.Rows.Cast<DataRow>()
                        .Select(row => string.Join(delimiter, columns.Select(c => row[c]))));

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Creates individual CSV files from the list of <see cref="System.Data.DataTable"/>s.
        /// </summary>
        /// <param name="T">The list of <see cref="DataTable"/>s to create a CSV files from.</param>
        /// <param name="folderPath">The folder path that the CSV files should be saved into.</param>
        /// <param name="delimiter">The delimiter to separate fields with. Default is a comma (,).</param>
        /// <returns>True if the files were created; False otherwise.</returns>
        /// <exception cref="DirectoryNotFoundException">the containing directories for the <paramref name="folderPath"/> were not found.</exception>
        public static bool ToCsvFiles(this List<DataTable> T, string folderPath, string delimiter = ",")
        {
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"Destination folder not found: {folderPath}");

            try
            {
                foreach (var table in T)
                {
                    var fileName = $"{table.TableName}.csv";
                    var path = Path.Combine(folderPath, fileName);
                    var columns = table.Columns.Cast<DataColumn>().ToArray();

                    var lines = (new[] { string.Join(delimiter, columns.Select(c => c.ColumnName)) })
                        .Concat(table.Rows.Cast<DataRow>()
                            .Select(row => string.Join(delimiter, columns.Select(c => row[c]))));

                    File.WriteAllLines(path, lines);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Serializes an object to an XML string.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="obj">An instance of an object to serialize.</param>
        /// <returns>An XML string representing the serialized object.</returns>
        public static string Serialize<T>(this T obj) where T : class
        {
            XmlSerializer _serializer = new XmlSerializer(obj.GetType());
            using StringWriter sw = new();
            _serializer.Serialize(sw, obj);
            return sw.ToString();
        }

        /// <summary>
        /// Deserializes an object from an XML string.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize to.</typeparam>
        /// <param name="xml">The XML string to deserialize.</param>
        /// <returns>The deserialized object or <c>null</c> if the deserialization failed.</returns>
        public static T? Deserialize<T>(this string xml) where T : class
        {
            XmlSerializer _serializer = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(xml))
            using (XmlReader reader = XmlReader.Create(sr))
            {
                if (_serializer.CanDeserialize(reader))
                    return (T?)_serializer.Deserialize(reader);
            }
            return null;
        }

        /// <summary>
        /// Get a value indicating if this object can be serialized via XML serialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns><c>true</c> if the object can be serialized; <c>false</c> otherwise.</returns>
        public static bool CanSerialize<T>(this T obj) where T : class
        {
            try
            {
                string _ = obj.Serialize();
            } catch (Exception) { return false; }
            return true;
        }

        private static readonly object SyncRoot = new object();

        //Implemented based on: https://josipmisko.com/posts/c-sharp-rename-dictionary-key
        /// <summary>
        /// Provides a thread-safe way to rename a key contained within a dictionary.
        /// <para>If the <paramref name="newKey"/> is the same as the <paramref name="oldKey"/> using the default equality operator,
        /// nothing is done.</para>
        /// </summary>
        /// <typeparam name="TKey">The <see cref="Type"/> of the key.</typeparam>
        /// <typeparam name="TValue">The <see cref="Type"/> of the value.</typeparam>
        /// <param name="dictionary">The dictionary to perform the rename on.</param>
        /// <param name="oldKey">The name of the old key.</param>
        /// <param name="newKey">The name to change <paramref name="oldKey"/> to.</param>
        /// <exception cref="ArgumentException">Throws if the <paramref name="dictionary"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Throws if the <paramref name="newKey"/> already exists in the dictionary.</exception>
        public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey oldKey, TKey newKey) where TKey : notnull
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(dictionary));

            if (EqualityComparer<TKey>.Default.Equals(oldKey, newKey))
                return;
            lock (SyncRoot)
            {
                if (dictionary.TryGetValue(oldKey, out TValue? value))
                {
                    if (dictionary.ContainsKey(newKey))
                        throw new ArgumentException("The new key already exists in the dictionary");

                    dictionary.Remove(oldKey);
                    dictionary.Add(newKey, value);
                }
            }
        }

        /// <summary>
        /// Get a value indicating if the string appears to be a base-64 encoded string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsBase64Encoded(this string value) => RegexGenerator.Base64Regex().IsMatch(value);

        /// <summary>
        /// Get a value indicating if the string appears to be a hex encoded string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsHexEncoded(this string value) => RegexGenerator.HexRegex().IsMatch(value);
    }
}
