// ConfigAttribute.cs : AetherUtils
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

namespace AetherUtils.Core.Attributes;

/// <summary>
///     Specifies the name of a property in a YAML configuration file, an optional comment description, and an optional default value to apply.
///     <example>
///         [Config("connectionString", "The connection string used for connecting to a database.", "host=;port=;user=;password=;database=;")]<br/>
///         public string ConnectionString { get; set; } = string.Empty;
///     </example>
///     <param name="name">The name of the property as it will appear in the YAML file.</param>
///     <param name="description">A description of the property that will appear as a comment above the property in YAML.</param>
///     <param name="defaultValue">The default value that should be applied to the property in the YAML file.</param>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ConfigAttribute(string name, string? description = null, object? defaultValue = null) : Attribute
{
    /// <summary>
    ///     The YAML name of the property in the configuration file.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    ///     An optional description that will be written as a comment above the config name in the YAML file.
    /// </summary>
    public string? Description { get; } = description;
    
    /// <summary>
    ///     An optional default value for this configuration option. Defaults to <c>null</c> if not specified.
    /// </summary>
    public object? DefaultValue { get; } = defaultValue;
}
