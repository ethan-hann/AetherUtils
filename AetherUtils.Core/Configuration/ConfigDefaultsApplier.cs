// ConfigDefaultApplier.cs : AetherUtils
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
using System.Collections;
using AetherUtils.Core.Attributes;
using AetherUtils.Core.Logging;

namespace AetherUtils.Core.Configuration;

/// <summary>
///     Applies default values defined in <see cref="ConfigAttribute" /> to config objects after deserialization.
/// </summary>
public static class ConfigDefaultsApplier
{
    /// <summary>
    ///     Applies default values to all properties marked with <see cref="ConfigAttribute" />
    ///     where the current value is null or a default primitive.
    /// </summary>
    /// <typeparam name="T">The config type.</typeparam>
    /// <param name="target">The config instance.</param>
    public static void ApplyDefaults<T>(T target) where T : class
    {
        ApplyToObject(target, []);
    }

    private static void ApplyToObject(object obj, HashSet<object> visited)
    {
        if (!visited.Add(obj)) return;

        var type = obj.GetType();
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var attr = prop.GetCustomAttribute<ConfigAttribute>();
            if (attr?.DefaultValue == null || !prop.CanWrite) continue;

            var currentVal = prop.GetValue(obj);
            var defaultVal = attr.DefaultValue;

            var needsAssignment =
                currentVal == null ||
                (prop.PropertyType.IsValueType && Equals(currentVal, Activator.CreateInstance(prop.PropertyType))) ||
                (currentVal is string s && string.IsNullOrWhiteSpace(s));

            if (needsAssignment)
            {
                try
                {
                    object? converted = null;

                    if (prop.PropertyType.IsGenericType)
                    {
                        var genericDef = prop.PropertyType.GetGenericTypeDefinition();

                        if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                        {
                            if (genericDef == typeof(Dictionary<,>) && defaultVal is IEnumerable dictionaryLike)
                            {
                                var args = prop.PropertyType.GetGenericArguments();
                                var keyType = args[0];
                                var valueType = args[1];
                                var dictType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
                                var dictInstance = Activator.CreateInstance(dictType) as IDictionary;

                                foreach (var entry in dictionaryLike)
                                {
                                    if (entry is not string kv || !kv.Contains('='))
                                        continue;
                                    
                                    var split = kv.Split('=', 2);
                                    var key = Convert.ChangeType(split[0], keyType);
                                    var value = Convert.ChangeType(split[1], valueType);
                                    dictInstance?.Add(key, value);
                                }
                                converted = dictInstance;
                            }
                            else
                            {
                                var elementType = prop.PropertyType.GetGenericArguments().FirstOrDefault();
                                if (elementType != null && defaultVal is IEnumerable enumerable)
                                {
                                    var listType = typeof(List<>).MakeGenericType(elementType);
                                    var listInstance = Activator.CreateInstance(listType) as IList;

                                    foreach (var item in enumerable)
                                    {
                                        listInstance?.Add(Convert.ChangeType(item, elementType));
                                    }

                                    converted = listInstance;
                                }
                            }
                        }
                        else if (genericDef == typeof(Nullable<>))
                        {
                            var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
                            if (underlyingType != null)
                            {
                                converted = Convert.ChangeType(defaultVal, underlyingType);
                            }
                        }
                    }

                    converted ??= Convert.ChangeType(defaultVal, prop.PropertyType);

                    prop.SetValue(obj, converted);
                }
                catch (Exception ex)
                {
                    AuLogger.GetCurrentLogger("ConfigDefaultsApplier").Error(ex, "Error creating default value for configuration property.");
                }
            }

            var nestedVal = prop.GetValue(obj);
            if (nestedVal != null && !prop.PropertyType.IsPrimitive && prop.PropertyType != typeof(string))
                ApplyToObject(nestedVal, visited);
        }
    }
}
