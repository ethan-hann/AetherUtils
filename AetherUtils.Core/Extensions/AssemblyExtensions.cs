using System.Drawing.Text;
using System.Reflection;
using AetherUtils.Core.Structs;

namespace AetherUtils.Core.Extensions;

/// <summary>
/// Provides extension methods for getting data from an <see cref="Assembly"/>.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Get the specified embedded resource from the <see cref="Assembly"/>.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/> to read the resource from.</param>
    /// <param name="resourceName">The name of the resource, including the namespace.</param>
    /// <returns>A string representing the resource, or <c>null</c> if the resource wasn't found.</returns>
    public static string? GetStringResource(this Assembly assembly, string resourceName)
    {
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) { return null; }

        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Get the specified embedded resource from the <see cref="Assembly"/> as a raw stream.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/> to read the resource from.</param>
    /// <param name="resourceName">The name of the resource, including the namespace.</param>
    /// <returns>A raw <see cref="Stream"/> representing the resource,
    /// or <c>null</c> if the resource wasn't found.</returns>
    public static Stream? GetResourceAsStream(this Assembly assembly, string resourceName) => 
        assembly.GetManifestResourceStream(resourceName);

    
    /// <summary>
    /// Get the specified embedded font resource from the <see cref="Assembly"/> and add it
    /// to the <see cref="PrivateFontCollection"/>.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/> to read the resource from.</param>
    /// <param name="resourceName">The name of the resource, including the namespace.</param>
    /// <param name="fontCollection">The <see cref="PrivateFontCollection"/> to add the read font to.</param>
    /// <returns>The modified <see cref="PrivateFontCollection"/> after the font has been added, or the original
    /// <see cref="PrivateFontCollection"/> if the font could not be added.</returns>
    public static PrivateFontCollection AddFontResource(this Assembly assembly, string resourceName, 
        PrivateFontCollection fontCollection)
    {
        ArgumentException.ThrowIfNullOrEmpty(resourceName, nameof(resourceName));
        ArgumentNullException.ThrowIfNull(fontCollection, nameof(fontCollection));
        
        var stream = assembly.GetResourceAsStream(resourceName);
        if (stream == null) return fontCollection;
        
        //Read the font data from the stream into a new array of bytes
        var fontData = new byte[stream.Length];
        _ = stream.Read(fontData, 0, (int)stream.Length);
        stream.Close();
        
        // Allocate memory for the font data and copy it
        var fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
        System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
        
        fontCollection.AddMemoryFont(fontPtr, fontData.Length);
        
        // Free the allocated memory
        System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);
        
        return fontCollection;
    }
}













