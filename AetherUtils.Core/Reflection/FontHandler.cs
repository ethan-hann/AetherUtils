using System.Drawing.Text;
using System.Reflection;
using AetherUtils.Core.Extensions;

namespace AetherUtils.Core.Reflection;

/// <summary>
/// <para>Handles loading, adding, and applying fonts from embedded resources to the active <see cref="Assembly"/>
/// and controls. This class cannot be used for loading fonts from disk. Font files must be included in the active
/// assembly and embedded.</para>
/// <para>Embedded fonts can be added to the internal font collection by using <see cref="AddFont"/>. Once added,
/// these fonts can be applied to controls using <see cref="ApplyFont"/>.</para>
/// <para>This class cannot be inherited or instantiated.</para>
/// </summary>
public sealed class FontHandler
{
    private readonly object _lock = new object();
    private FontHandler? _instance = null;
    private readonly Dictionary<string, string> _fontDict = new();
    
    private PrivateFontCollection _privateFonts = new();
    
    /// <summary>
    /// Get the single instance of the <see cref="FontLoader"/>.
    /// </summary>
    public FontHandler Instance
    {
        get
        {
            lock (_lock)
            {
                return _instance ??= new FontHandler();
            }
        }
    }

    private FontHandler() { }

    /// <summary>
    /// Add the font specified to the font collection.
    /// </summary>
    /// <param name="fontName">The font family name.</param>
    /// <param name="fontResourcePath">The fully qualified path to the embedded resource.</param>
    public void AddFont(string fontName, string fontResourcePath)
    {
        if (_fontDict.ContainsValue(fontResourcePath)) return;
        
        _fontDict[fontName] = fontResourcePath;
        _privateFonts = Assembly.GetExecutingAssembly().AddFontResource(fontResourcePath, _privateFonts);
    }

    /// <summary>
    /// Apply the specified font family to the control.
    /// </summary>
    /// <param name="control">The <see cref="Control"/> to apply the font to.</param>
    /// <param name="fontName">The name of the font family.</param>
    /// <param name="fontSize">The size of the font to be applied.</param>
    /// <param name="style">The <see cref="FontStyle"/> of the font to be applied.</param>
    /// <returns><c>true</c> if the font was applied; <c>false</c> otherwise.</returns>
    public bool ApplyFont(Control control, string fontName, float fontSize, FontStyle style)
    {
        if (_privateFonts.Families.Length <= 0) return false;

        var customFont = new Font(_privateFonts.Families.First(f => f.Name.Equals(fontName)), fontSize, style);
        control.ApplyFont(customFont);
        return true;
    }
}