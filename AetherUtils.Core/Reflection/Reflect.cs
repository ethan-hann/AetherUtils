using System.Collections;
using System.Reflection;

namespace AetherUtils.Core.Reflection
{
    /// <summary>
    /// Contains methods to help with runtime reflection of classes, types, and attributes.
    /// </summary>
    public static class Reflect
    {
        /// <summary>
        /// Indicates whether or not the specified type is a <see cref="List{T}"/>.
        /// </summary>
        /// <param name="type">The type to query.</param>
        /// <returns><c>true</c> if the type is a <see cref="List{T}"/>; <c>false</c> otherwise</returns>
        public static bool IsList(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            if (typeof(IList).IsAssignableFrom(type))
                return true;
            foreach (var it in type.GetInterfaces())
                if (it.IsGenericType && typeof(IList<>) == it.GetGenericTypeDefinition())
                    return true;
            return false;
        }

        /// <summary>
        /// Retrieves the collection element type from this type.
        /// </summary>
        /// <param name="type">The type to query.</param>
        /// <returns>The element type of the collection or <c>null</c> if the type was not a collection.
        /// </returns>
        public static Type? GetCollectionElementType(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            // first try the generic way
            // this is easy, just query the IEnumerable<T> interface for its generic parameter
            var etype = typeof(IEnumerable<>);
            foreach (var bt in type.GetInterfaces())
                if (bt.IsGenericType && bt.GetGenericTypeDefinition() == etype)
                    return bt.GetGenericArguments()[0];

            // now try the non-generic way

            // if it's a dictionary we always return DictionaryEntry
            if (typeof(IDictionary).IsAssignableFrom(type))
                return typeof(DictionaryEntry);

            // if it's a list we look for an Item property with an int index parameter
            // where the property type is anything but object
            if (typeof(IList).IsAssignableFrom(type))
            {
                foreach (var prop in type.GetProperties())
                {
                    if ("Item" == prop.Name && typeof(object) != prop.PropertyType)
                    {
                        var ipa = prop.GetIndexParameters();
                        if (1 == ipa.Length && typeof(int) == ipa[0].ParameterType)
                        {
                            return prop.PropertyType;
                        }
                    }
                }
            }

            // if it's a collection, we look for an Add() method whose parameter is 
            // anything but object
            if (typeof(ICollection).IsAssignableFrom(type))
            {
                foreach (var meth in type.GetMethods())
                {
                    if ("Add" == meth.Name)
                    {
                        var pa = meth.GetParameters();
                        if (1 == pa.Length && typeof(object) != pa[0].ParameterType)
                            return pa[0].ParameterType;
                    }
                }
            }
            if (typeof(IEnumerable).IsAssignableFrom(type))
                return typeof(object);
            return null;
        }

        //SEE: https://stackoverflow.com/questions/53029972/recursively-get-properties-marked-with-an-attribute
        /// <summary>
        /// Retrieve all of the <see cref="Type"/>, <see cref="PropertyInfo"/>, <see cref="Attribute"/>, and <see cref="object"/>s 
        /// related to the custom attribute <typeparamref name="T"/> specified, recursively.
        /// </summary>
        /// <typeparam name="T">The custom attribute to search for.</typeparam>
        /// <param name="instance">The initial <see cref="object"/> instance to start the search on.</param>
        /// <param name="type">The initial <see cref="Type"/> to start the search on.</param>
        /// <param name="visited">Defaults to <c>null</c>. Used to maintain a <see cref="HashSet{T}"/> of already visited types while searching.</param>
        /// <returns><para>
        /// An <see cref="IEnumerable"/> of tuples:<br/>
        /// <c>(</c><see cref="Type"/>, <see cref="PropertyInfo"/>, <see cref="Attribute"/>, <see cref="object"/><c>)</c>
        /// </para></returns>
        public static IEnumerable<(Type Class, PropertyInfo Property, Attribute Attribute, object? Instance)> GetAttributesRecurse<T>(object? instance, Type type, HashSet<Type>? visited = null) where T : Attribute
        {
            // Keep track of where we have been.
            visited ??= []; //Shorthand for visited = visited ?? new HashSet<Type>();

            //If we've been here before, the type is System.String, or the type is a list, yield to prevent StackOverflow error.
            if (type.FullName != null && (!visited.Add(type) || type.FullName.Equals("System.String") || type.IsPrimitive || IsList(type)))
                yield break;

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var customAttribute = prop.GetCustomAttribute<T>(true);
                if (customAttribute != null)
                    // The Attribute exists, let's yield and return.
                    yield return (type, prop, customAttribute, instance);

                // Recurse the property's type as well.
                foreach (var result in GetAttributesRecurse<T>(prop.GetValue(instance), prop.PropertyType, visited))
                    yield return result;
            }
        }
    }
}
