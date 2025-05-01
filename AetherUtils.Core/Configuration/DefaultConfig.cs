// DefaultConfig.cs : AetherUtils
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

using AetherUtils.Core.Attributes;
using JetBrains.Annotations;

namespace AetherUtils.Core.Configuration;

/// <summary>
///     A <a href="https://en.wikipedia.org/wiki/Data_transfer_object">DTO</a> representing the default, bare configuration
///     of a new application. This class can be used as is,
///     or a new class can be created to store the configuration for an application.<br />
///     <para>
///         If a new class is needed, it must be a <a href="https://en.wikipedia.org/wiki/Data_transfer_object">DTO</a>
///         and it's properties should be marked with <see cref="ConfigAttribute" /> attributes in order to be saved and
///         loaded from disk by a <see cref="ConfigManager{T}" />.
///     </para>
///     <remarks>
///         This class can contain instances of other <a href="https://en.wikipedia.org/wiki/Data_transfer_object">DTO</a>
///         classes so long as those classes also have the <see cref="ConfigAttribute" /> on their properties.
///     </remarks>
/// </summary>
public sealed class DefaultConfig
{
    /// <summary>
    ///     The connection string used for connecting to a database.
    /// </summary>
    [Config("connectionString", "The connection string used for connecting to a database.", "host=;port=;user=;password=;database=;")]
    [UsedImplicitly]
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    ///     The full path to a valid license file for an application.
    /// </summary>
    [Config("licenseFile", "The full path to a valid license file for an application.", "")]
    [UsedImplicitly]
    public string LicenseFile { get; set; } = string.Empty;

    /// <summary>
    ///     A collection of options used for logging.
    /// </summary>
    [Config("logOptions", "A collection of options used for logging.")]
    [UsedImplicitly]
    public LogOptions LogOptions { get; set; } = new();
}