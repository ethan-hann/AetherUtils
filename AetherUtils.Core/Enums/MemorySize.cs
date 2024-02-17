using System.ComponentModel;

namespace AetherUtils.Core.Enums;

/// <summary>
/// Represents the value in bytes of various memory sizes; uses <c>1024</c> as the conversion factor.
/// </summary>
internal enum MemorySize : ulong
{
    /// <summary>
    /// Represents 1 Byte.
    /// </summary>
    [Description("Byte")]
    Byte = 1,
    
    /// <summary>
    /// Represents 1 Kilobyte.
    /// </summary>
    [Description("KB")]
    KiloByte = Byte * 1024,
    
    /// <summary>
    /// Represents 1 MegaByte.
    /// </summary>
    [Description("MB")]
    MegaByte = KiloByte * 1024,
    
    /// <summary>
    /// Represents 1 GigaByte.
    /// </summary>
    [Description("GB")]
    GigaByte = MegaByte * 1024,
    
    /// <summary>
    /// Represents 1 TerraByte.
    /// </summary>
    [Description("TB")]
    TerraByte = GigaByte * 1024,
    
    /// <summary>
    /// Represents 1 PetaByte.
    /// </summary>
    [Description("PB")]
    PetaByte = TerraByte * 1024,
    
    /// <summary>
    /// Represents 1 ExaByte.
    /// </summary>
    [Description("EB")]
    ExaByte = PetaByte * 1024
}