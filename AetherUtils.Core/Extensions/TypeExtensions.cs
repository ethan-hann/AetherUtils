// TypeExtensions.cs : AetherUtils
// Copyright (C) 2025  Ethan Hann
// 
// MIT License
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.CodeDom;
using System.Xml.Serialization;
using AetherUtils.Core.Files;
using JetBrains.Annotations;
using Microsoft.CSharp;

namespace AetherUtils.Core.Extensions;

/// <summary>
///     Provides extension methods for manipulating generic object types.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    ///     Get the friendly, displayable name for the <see cref="Type" />.
    /// </summary>
    /// <param name="type">The <see cref="Type" /> to get the name of.</param>
    /// <returns>The friendly name for the <see cref="Type" />.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="type" /> was <c>null</c>.</exception>
    [UsedImplicitly]
    public static string GetFriendlyName(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type, nameof(type));

        using var provider = new CSharpCodeProvider();
        var typeRef = new CodeTypeReference(type);
        return provider.GetTypeOutput(typeRef);
    }

    /// <summary>
    ///     Serializes a .NET object to an XML string.
    /// </summary>
    /// <param name="obj">An instance of <typeparamref name="T" /> to serialize.</param>
    /// <typeparam name="T">The <see cref="Type" /> of the object to serialize.</typeparam>
    /// <returns>An XML string representing the serialized object, <paramref name="obj" />.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="obj" /> was <c>null</c>.</exception>
    public static string SerializeXml<T>(this T obj) where T : class
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));

        var serializer = new XmlSerializer(obj.GetType());
        using StringWriter sw = new();
        serializer.Serialize(sw, obj);
        return sw.ToString();
    }

    /// <summary>
    ///     Serializes a .NET object to a JSON string.
    /// </summary>
    /// <param name="obj">An instance of <typeparamref name="T" /> to serialize.</param>
    /// <typeparam name="T">The <see cref="Type" /> of the object to serialize.</typeparam>
    /// <returns>A JSON string representing the serialized object, <paramref name="obj" />.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="obj" /> was <c>null</c>.</exception>
    public static string SerializeJson<T>(this T obj) where T : class
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));

        var serializer = new Json<T>();
        return serializer.ToJson(obj);
    }

    /// <summary>
    ///     Get a value indicating if this object can be serialized via XML serialization.
    /// </summary>
    /// <param name="obj">The instance of the object with type <typeparamref name="T" />.</param>
    /// <typeparam name="T">The <see cref="Type" /> of the object to check.</typeparam>
    /// <returns><c>true</c> if the object can be serialized; <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="obj" /> was <c>null</c>.</exception>
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
    ///     Get a value indicating if this object can be serialized via JSON serializion.
    /// </summary>
    /// <param name="obj">The instance of the object with type <typeparamref name="T" />.</param>
    /// <typeparam name="T">The <see cref="Type" /> of the object to check.</typeparam>
    /// <returns><c>true</c> if the object can be serialized; <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="obj" /> was <c>null</c>.</exception>
    [UsedImplicitly]
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