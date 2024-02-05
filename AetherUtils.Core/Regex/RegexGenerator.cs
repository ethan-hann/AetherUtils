using System.Text.RegularExpressions;

namespace AetherUtils.Core.RegEx
{
    public static partial class RegexGenerator
    {
        /// <summary>
        /// Regex source generator for a valid absolute file path on Windows.
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^(([a-zA-Z]:)|(\))(\{1}|((\{1})[^\]([^/:*?<>""|]*))+)$", RegexOptions.CultureInvariant, 1000)]
        public static partial Regex PathRegex();

        /// <summary>
        /// Regex source generator for a BASE64 string.
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$", RegexOptions.CultureInvariant, 1000)]
        public static partial Regex Base64Regex();

        /// <summary>
        /// Regex source generator for a HEX string.
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^(0x|0X)?[a-fA-F0-9]+$", RegexOptions.CultureInvariant, 1000)]
        public static partial Regex HexRegex();
    }
}
