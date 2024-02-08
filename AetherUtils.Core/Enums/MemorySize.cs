using System.ComponentModel;

namespace AetherUtils.Core.Enums;

/// <summary>
/// Represents the value in bytes of various memory sizes; uses <c>1024</c> as the conversion factor.
/// </summary>
public enum MemorySize : ulong
{
    [Description("Byte")]
    Byte = 1,
    
    [Description("KB")]
    KiloByte = Byte * 1024,
    
    [Description("MB")]
    MegaByte = KiloByte * 1024,
    
    [Description("GB")]
    GigaByte = MegaByte * 1024,
    
    [Description("TB")]
    TerraByte = GigaByte * 1024,
    
    [Description("PB")]
    PetaByte = TerraByte * 1024,
    
    [Description("EB")]
    ExaByte = PetaByte * 1024
}