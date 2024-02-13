using Microsoft.CSharp;
using System.CodeDom;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;
using AetherUtils.Core.Files;

namespace AetherUtils.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for manipulating generic object types.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Get the friendly, displayable name for the <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get the name of.</param>
        /// <returns>The friendly name for the <see cref="Type"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="type"/> was <c>null</c>.</exception>
        public static string GetFriendlyName(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));
            
            using var provider = new CSharpCodeProvider();
            var typeRef = new CodeTypeReference(type);
            return provider.GetTypeOutput(typeRef);
        }
        
        /// <summary>
        /// Serializes a .NET object to an XML string.
        /// </summary>
        /// <param name="obj">An instance of <typeparamref name="T"/> to serialize.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the object to serialize.</typeparam>
        /// <returns>An XML string representing the serialized object, <paramref name="obj"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="obj"/> was <c>null</c>.</exception>
        public static string SerializeXml<T>(this T obj) where T : class
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));
            
            var serializer = new XmlSerializer(obj.GetType());
            using StringWriter sw = new();
            serializer.Serialize(sw, obj);
            return sw.ToString();
        }

        /// <summary>
        /// Serializes a .NET object to a JSON string.
        /// </summary>
        /// <param name="obj">An instance of <typeparamref name="T"/> to serialize.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the object to serialize.</typeparam>
        /// <returns>A JSON string representing the serialized object, <paramref name="obj"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="obj"/> was <c>null</c>.</exception>
        public static string SerializeJson<T>(this T obj) where T : class
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));

            var serializer = new Json<T>();
            return serializer.ToJson(obj);
        }
        
        /// <summary>
        /// Get a value indicating if this object can be serialized via XML serialization.
        /// </summary>
        /// <param name="obj">The instance of the object with type <typeparamref name="T"/>.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the object to check.</typeparam>
        /// <returns><c>true</c> if the object can be serialized; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="obj"/> was <c>null</c>.</exception>
        public static bool CanSerializeXml<T>(this T obj) where T : class
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));
            
            try
            {
                obj.SerializeXml();
                return true;
            }
            catch (Exception) { return false; }
        }

        /// <summary>
        /// Get a value indicating if this object can be serialized via JSON serializion.
        /// </summary>
        /// <param name="obj">The instance of the object with type <typeparamref name="T"/>.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the object to check.</typeparam>
        /// <returns><c>true</c> if the object can be serialized; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="obj"/> was <c>null</c>.</exception>
        public static bool CanSerializeJson<T>(this T obj) where T : class
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));
            
            try
            {
                var s = obj.SerializeJson();
                return s.Equals(string.Empty);
            }
            catch (Exception) { return false; }
        }
    }
}
