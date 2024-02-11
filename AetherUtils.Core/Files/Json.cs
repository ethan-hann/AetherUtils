using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace AetherUtils.Core.Files
{
    /// <summary>
    /// Implements serializing and de-serializing generic object types to/from JSON files.
    /// </summary>
    /// <remarks><typeparamref name="T"/> must support JSON serialization.</remarks>
    /// <typeparam name="T">The type of object to serialize/deserialize.</typeparam>
    public sealed class Json<T> where T : class
    {
        private readonly JsonSerializer _serializer;
        private StringBuilder _stringBuilder;
        private StringWriter _stringWriter;

        public Json()
        {
            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
            
            _serializer = JsonSerializer.Create(settings);
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
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public bool SaveJson(string filePath, T obj)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            filePath = FileHelper.ExpandPath(filePath);

            string json = ToJson(obj);
            FileHelper.SaveFile(filePath, json, false);
            
            return FileHelper.DoesFileExist(filePath, false);
        }

        public string ToJson(T obj)
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));
            using JsonWriter writer = new JsonTextWriter(_stringWriter);
            _serializer.Serialize(writer, obj);
            var json = _stringWriter.ToString();
            
            ResetWriters();

            return json;
        }

        public T? FromJson(string json)
        {
            ArgumentException.ThrowIfNullOrEmpty(json, nameof(json));
            using var reader = new JsonTextReader(new StringReader(json));
            var obj = _serializer.Deserialize<T>(reader);
            ResetWriters();

            return obj;
        }

        /// <summary>
        /// Deserialize and load a .NET object from a JSON file.
        /// </summary>
        /// <param name="filePath">The file to load.</param>
        /// <returns>The <typeparamref name="T"/> object, or <c>null</c> if the object could not be deserialized.</returns>
        /// <exception cref="ArgumentException">If <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        /// <exception cref="FileNotFoundException">If the <see cref="filePath"/> did not exist.</exception>
        public T? LoadJson(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            filePath = FileHelper.ExpandPath(filePath);

            if (!FileHelper.DoesFileExist(filePath, false))
                throw new FileNotFoundException("File was not found");

            var json = FileHelper.OpenFile(filePath);
            using var reader = new JsonTextReader(new StringReader(json));
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
