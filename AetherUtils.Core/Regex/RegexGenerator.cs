using System.Text.RegularExpressions;

namespace AetherUtils.Core.Regex
{
    public static partial class RegexGenerator
    {
        /// <summary>
        /// Regex source generator for a valid absolute file path on Windows.
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^(([a-zA-Z]:)|(\))(\{1}|((\{1})[^\]([^/:*?<>""|]*))+)$", RegexOptions.CultureInvariant, 1000)]
        public static partial System.Text.RegularExpressions.Regex PathRegex();
    }
}
