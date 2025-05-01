// YamlCommentInjector.cs : AetherUtils
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

using System.Reflection;
using AetherUtils.Core.Attributes;
using YamlDotNet.Serialization;

namespace AetherUtils.Core.Configuration;

/// <summary>
///     Applies YAML comments to properties marked with a <see cref="ConfigAttribute" /> by generating
///     <see cref="YamlMemberAttribute" /> overrides dynamically.
///     This allows <see cref="ConfigAttribute.Description"/> to appear as YAML comments without requiring per-property annotations.
/// </summary>
public static class YamlCommentInjector
{
    /// <summary>
    ///     Applies all attribute overrides to a <see cref="SerializerBuilder" /> for the given config type.
    /// </summary>
    public static SerializerBuilder ApplyToBuilder<T>(SerializerBuilder builder) where T : class
    {
        var visited = new HashSet<Type>();
        var configType = typeof(T);

        ApplyOverridesRecursive(configType, builder, visited);
        return builder;
    }

    private static void ApplyOverridesRecursive(Type type, SerializerBuilder builder, HashSet<Type> visited)
    {
        if (visited.Contains(type) || type.Namespace?.StartsWith("System") == true)
            return;

        visited.Add(type);

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var configAttr = prop.GetCustomAttribute<ConfigAttribute>();
            if (configAttr?.Description != null)
            {
                var yamlAttr = new YamlMemberAttribute { Description = configAttr.Description };
                builder.WithAttributeOverride(type, prop.Name, yamlAttr);
            }

            var propType = prop.PropertyType;

            if (propType.IsClass && propType != typeof(string))
            {
                ApplyOverridesRecursive(propType, builder, visited);
            }

            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(propType) && propType.IsGenericType)
            {
                var elementType = propType.GetGenericArguments()[0];
                ApplyOverridesRecursive(elementType, builder, visited);
            }
        }
    }
} 
