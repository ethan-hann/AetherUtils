using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        public void SaveJSON(string filePath, T obj)
        {
            try
            {
                using (JsonWriter writer = new JsonTextWriter(_stringWriter))
                {
                    _serializer.Serialize(writer, obj);
                    FileHelper.SaveFile(filePath, _stringWriter.ToString());
                }

                ResetWriters();
            } catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        public T? LoadJSON(string filePath)
        {
            T? obj = null;
            try
            {
                string json = FileHelper.OpenFile(filePath);
                using JsonTextReader reader = new JsonTextReader(new StringReader(json));
                obj = _serializer.Deserialize<T>(reader);
            } catch (Exception ex) { Debug.WriteLine(ex.Message); }

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
