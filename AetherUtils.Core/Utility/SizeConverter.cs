using AetherUtils.Core.Structs;

namespace AetherUtils.Core.Utility
{
    public static class SizeConverter
    {
        private static readonly List<ReadOnlyPair<string, long>> MemorySizes =
        [
            new ReadOnlyPair<string, long>("Byte", 1),
            new ReadOnlyPair<string, long>("KiloByte", 1024)
        ];

    }
}
