using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void SaveXML(string filePath, T obj)
        {
            filePath = FileHelper.ExpandPath(filePath);
            FileHelper.CreateDirectories(filePath, false);
            using XmlWriter writer = XmlWriter.Create(filePath);
            try
            {
                _serializer.Serialize(writer, obj);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            finally { writer.Close(); }
        }

        public T? LoadXML(string filePath)
        {
            filePath = FileHelper.ExpandPath(filePath);
            using XmlReader reader = XmlReader.Create(filePath);
            try
            {
                return (T?)_serializer.Deserialize(reader);
            } catch (Exception ex) { Debug.WriteLine(ex.Message); return null; }
            finally { reader.Close(); }
        }
    }
}
