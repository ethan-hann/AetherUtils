// // Xml.cs : AetherUtils
// // Copyright (C) 2025  Ethan Hann
// //
// // MIT License
// // Permission is hereby granted, free of charge, to any person obtaining a copy
// // of this software and associated documentation files (the "Software"), to deal
// // in the Software without restriction, including without limitation the rights
// // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// // copies of the Software, and to permit persons to whom the Software is
// // furnished to do so, subject to the following conditions:
// //
// // The above copyright notice and this permission notice shall be included in all
// // copies or substantial portions of the Software.
// //
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// // SOFTWARE.

using AetherUtils.Core.Extensions;
using JetBrains.Annotations;

namespace AetherUtils.Core.Files;

/// <summary>
///     Implements serializing and de-serializing generic object types to/from XML files.
/// </summary>
/// <remarks><typeparamref name="T" /> must support XML serialization.</remarks>
/// <typeparam name="T">The type of object to serialize/deserialize.</typeparam>
[UsedImplicitly]
public sealed class Xml<T> where T : class
{
    /// <summary>
    ///     Serialize an object of type <typeparamref name="T" /> to an XML string and save to a file.
    ///     <para>If the file already exists, it is overwritten.</para>
    /// </summary>
    /// <param name="filePath">The file to save.</param>
    /// <param name="obj">The object to serialize and save.</param>
    /// <returns><c>true</c> if the object was serialized and the file was saved; <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentException">If the <paramref name="filePath" /> was <c>null</c> or empty.</exception>
    /// <exception cref="ArgumentNullException">If the <paramref name="obj" /> was <c>null</c>.</exception>
    [UsedImplicitly]
    public bool SaveXml(string filePath, T obj)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));

        filePath = FileHelper.ExpandPath(filePath);

        if (!obj.CanSerializeXml())
            return false;

        var xml = obj.SerializeXml();
        FileHelper.SaveFile(filePath, xml, false);

        return FileHelper.DoesFileExist(filePath, false);
    }

    /// <summary>
    ///     Deserialize and load a .NET object from an XML file.
    /// </summary>
    /// <param name="filePath">The file to load.</param>
    /// <returns>The <typeparamref name="T" /> object, or <c>null</c> if the object could not be deserialized.</returns>
    /// <exception cref="ArgumentException">If the <paramref name="filePath" /> was <c>null</c> or empty.</exception>
    [UsedImplicitly]
    public T? LoadXml(string filePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

        var xml = FileHelper.OpenFile(filePath);
        return xml.DeserializeXml<T>();
    }
}