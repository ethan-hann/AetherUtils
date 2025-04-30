// RegexGenerator.cs : AetherUtils
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

using System.Text.RegularExpressions;

namespace AetherUtils.Core.RegEx;

/// <summary>
///     Represents various regular expression generators.
/// </summary>
public static partial class RegexGenerator
{
    /// <summary>
    ///     Regex source generator for a valid absolute file path on Windows.
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"^(([a-zA-Z]:)|(\))(\{1}|((\{1})[^\]([^/:*?<>""|]*))+)$",
        RegexOptions.CultureInvariant, 1000)]
    public static partial Regex PathRegex();

    [GeneratedRegex(@"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$",
        RegexOptions.CultureInvariant, 1000)]
    public static partial Regex EmailRegex();

    /// <summary>
    ///     Regex source generator for a BASE64 string.
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$",
        RegexOptions.CultureInvariant, 1000)]
    public static partial Regex Base64Regex();

    /// <summary>
    ///     Regex source generator for a HEX string.
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"^(0x|0X)?[a-fA-F0-9]+$",
        RegexOptions.CultureInvariant, 1000)]
    public static partial Regex HexRegex();
}