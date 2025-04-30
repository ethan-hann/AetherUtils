// IFluentInterface.cs : AetherUtils
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

using System.ComponentModel;
using JetBrains.Annotations;

namespace AetherUtils.Core.Interfaces;

/// <summary>
///     Interface that is used to build fluent interfaces and
///     hides methods declared by <see cref="object" /> from IntelliSense.
/// </summary>
/// <remarks>
///     Code that consumes implementations of this interface should expect one of two things:
///     <list type="number">
///         <item>
///             When referencing the interface from within the same solution (project reference),
///             you will still see the methods this interface is meant to hide.
///         </item>
///         <item>
///             When referencing the interface through the compiled output assembly (external reference),
///             the standard Object methods will be hidden as intended.
///         </item>
///     </list>
///     See <a href="https://dotnettutorials.net/lesson/fluent-interface-design-pattern/">Fluent Interface</a>
///     for more information.
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IFluentInterface
{
    /// <summary>
    ///     Get the <see cref="Type" /> of this object.
    /// </summary>
    /// <returns>The <see cref="Type" /> definition of this object.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [UsedImplicitly]
    Type GetType();

    /// <summary>
    ///     Get a unique hash-code for this object.
    /// </summary>
    /// <returns>An integer.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [UsedImplicitly]
    int GetHashCode();

    /// <summary>
    ///     Get a human-readable string representing this object.
    /// </summary>
    /// <returns>A string.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [UsedImplicitly]
    string? ToString();

    /// <summary>
    ///     Check for equality between this object and <paramref name="obj" />.
    /// </summary>
    /// <param name="obj">The object to check against.</param>
    /// <returns><c>true</c> if the two objects are equal; <c>false</c> otherwise.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [UsedImplicitly]
    bool Equals(object obj);
}