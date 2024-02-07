using AetherUtils.Core.Extensions;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace AetherUtils.Core.Files
{
    /// <summary>
    /// Implements serializing and de-serializing XML files to/from generic object types.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize/deserialize.</typeparam>
    public sealed class Xml<T> where T : class
    {
        private readonly XmlSerializer _serializer;

        public Xml()
        {
            _serializer = new XmlSerializer(typeof(T));
        }

        /// <summary>
        /// Serialize a .NET object, <typeparamref name="T"/>, to an XML string and save to a file.
        /// <para>If the file already exists, it is overwritten.</para>
        /// </summary>
        /// <param name="filePath">The file to save.</param>
        /// <param name="obj">The .NET object to serialize and save.</param>
        /// <returns><c>true</c> if the object was serialized and the file was saved; <c>false</c> otherwise.</returns>
        public bool SaveXML(string filePath, T obj)
        {
            filePath = FileHelper.ExpandPath(filePath);
            FileHelper.CreateDirectories(filePath, false);

            if (!obj.CanSerialize())
                return false;

            string xml = obj.Serialize();
            FileHelper.SaveFile(filePath, xml);

            return true;
        }

        /// <summary>
        /// Deserialize and load a .NET object from an XML file.
        /// </summary>
        /// <param name="filePath">The file to load.</param>
        /// <returns>The <typeparamref name="T"/> object, or <c>null</c> if the object could not be deserialized.</returns>
        public T? LoadXML(string filePath)
        {
            string xml = FileHelper.OpenFile(filePath, true);
            return xml.Deserialize<T>();
        }
    }
}
