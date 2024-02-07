using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace AetherUtils.Core.Files
{
    /// <summary>
    /// Implements serializing and de-serializing JSON files to/from generic object types.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize/deserialize.</typeparam>
    public sealed class Json<T> where T : class
    {
        private readonly JsonSerializerSettings _settings;
        private readonly JsonSerializer _serializer;
        private StringBuilder _stringBuilder;
        private StringWriter _stringWriter;

        public Json()
        {
            _settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
            _serializer = JsonSerializer.Create(_settings);
            _stringBuilder = new StringBuilder();
            _stringWriter = new StringWriter(_stringBuilder);
        }

        /// <summary>
        /// Serialize a .NET object, <typeparamref name="T"/>, to a JSON string and save to a file.
        /// <para>If the file already exists, it is overwritten.</para>
        /// </summary>
        /// <param name="filePath">The file to save.</param>
        /// <param name="obj">The .NET object to serialize and save.</param>
        /// <returns><c>true</c> if the object was serialized and the file was saved; <c>false</c> otherwise.</returns>
        public bool SaveJson(string filePath, T obj)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            string newPath = FileHelper.ExpandPath(filePath);
            
            using JsonWriter writer = new JsonTextWriter(_stringWriter);
            _serializer.Serialize(writer, obj);
            FileHelper.SaveFile(newPath, _stringWriter.ToString(), false);

            ResetWriters();
            return FileHelper.DoesFileExist(newPath, false);
        }

        /// <summary>
        /// Deserialize and load a .NET object from a JSON file.
        /// </summary>
        /// <param name="filePath">The file to load.</param>
        /// <returns>The <typeparamref name="T"/> object, or <c>null</c> if the object could not be deserialized.</returns>
        public T? LoadJson(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            string newPath = FileHelper.ExpandPath(filePath);

            if (!FileHelper.DoesFileExist(newPath, false))
                throw new FileNotFoundException("File was not found");

            string json = FileHelper.OpenFile(newPath);
            using JsonTextReader reader = new JsonTextReader(new StringReader(json));
            var obj = _serializer.Deserialize<T>(reader);
            ResetWriters();
            
            return obj;
        }

        private void ResetWriters()
        {
            _stringBuilder = _stringBuilder.Clear();
            _stringWriter = new StringWriter(_stringBuilder);
        }
    }
}
