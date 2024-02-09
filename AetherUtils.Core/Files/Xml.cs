using AetherUtils.Core.Extensions;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace AetherUtils.Core.Files
{
    /// <summary>
    /// Implements serializing and de-serializing generic object types to/from XML files.
    /// </summary>
    /// <remarks><typeparamref name="T"/> must support XML serialization.</remarks>
    /// <typeparam name="T">The type of object to serialize/deserialize.</typeparam>
    public sealed class Xml<T> where T : class
    {
        /// <summary>
        /// Serialize an object of type <typeparamref name="T"/> to an XML string and save to a file.
        /// <para>If the file already exists, it is overwritten.</para>
        /// </summary>
        /// <param name="filePath">The file to save.</param>
        /// <param name="obj">The object to serialize and save.</param>
        /// <returns><c>true</c> if the object was serialized and the file was saved; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">If the <paramref name="obj"/> was <c>null</c>.</exception>
        public bool SaveXml(string filePath, T obj)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));
            
            filePath = FileHelper.ExpandPath(filePath);

            if (!obj.CanSerialize())
                return false;

            var xml = obj.Serialize();
            FileHelper.SaveFile(filePath, xml, false);

            return FileHelper.DoesFileExist(filePath, false);
        }

        /// <summary>
        /// Deserialize and load a .NET object from an XML file.
        /// </summary>
        /// <param name="filePath">The file to load.</param>
        /// <returns>The <typeparamref name="T"/> object, or <c>null</c> if the object could not be deserialized.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public T? LoadXml(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            var xml = FileHelper.OpenFile(filePath);
            return xml.Deserialize<T>();
        }
    }
}
