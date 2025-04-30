// // EnumExtensions.cs : AetherUtils
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

using System.ComponentModel;

namespace AetherUtils.Core.Extensions;

/// <summary>
///     Provides extension methods for manipulating <see cref="Enum" /> objects.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    ///     Get the string associated with a <see cref="DescriptionAttribute" /> attribute on an <see cref="Enum" /> value.
    /// </summary>
    /// <param name="val">The Enum value to get the description string of.</param>
    /// <typeparam name="T">The Enum type to get the attributes of.</typeparam>
    /// <returns>The description string or <see cref="string.Empty" /> if no <see cref="DescriptionAttribute" /> was found.</returns>
    public static string ToDescriptionString<T>(this T val) where T : Enum
    {
        if (val.Equals(null)) return string.Empty;
        try
        {
            var type = val.GetType();
            var field = type.GetField(val.ToString());

            if (field == null || field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    is not DescriptionAttribute[] attributes) return string.Empty;

            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
        catch (Exception) { return string.Empty; }
    }
}