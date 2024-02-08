using System.ComponentModel;

namespace AetherUtils.Core.Extensions;

/// <summary>
/// Provides extension methods for manipulating <see cref="Enum"/> objects.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Get the string associated with a <see cref="DescriptionAttribute"/> attribute on an <see cref="Enum"/> value.
    /// </summary>
    /// <param name="val">The Enum value to get the description string of.</param>
    /// <typeparam name="T">The Enum type to get the attributes of.</typeparam>
    /// <returns>The description string or <see cref="string.Empty"/> if no <see cref="DescriptionAttribute"/> was found.</returns>
    public static string ToDescriptionString<T>(this T val) where T : Enum
    {
        if (val.Equals(null)) return string.Empty;
        try
        {
            var type = val.GetType();
            var field = type.GetField(val.ToString());

            if (field == null) return string.Empty;

            if (field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                is not DescriptionAttribute[] attributes) return string.Empty;

            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
        catch (Exception) { return string.Empty; }
    }
}