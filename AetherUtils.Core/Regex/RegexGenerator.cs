using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
