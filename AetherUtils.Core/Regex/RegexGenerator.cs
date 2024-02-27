using System.Text.RegularExpressions;
using AetherUtils.Core.Security.Passwords;

namespace AetherUtils.Core.RegEx
{
    /// <summary>
    /// Represents various regular expression generators.
    /// </summary>
    public static partial class RegexGenerator
    {
        /// <summary>
        /// Regex source generator for a valid absolute file path on Windows.
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^(([a-zA-Z]:)|(\))(\{1}|((\{1})[^\]([^/:*?<>""|]*))+)$", 
            RegexOptions.CultureInvariant, 1000)]
        public static partial Regex PathRegex();

        [GeneratedRegex(@"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", 
            RegexOptions.CultureInvariant, 1000)]
        public static partial Regex EmailRegex();

        /// <summary>
        /// Regex source generator for a BASE64 string.
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$", 
            RegexOptions.CultureInvariant, 1000)]
        public static partial Regex Base64Regex();

        /// <summary>
        /// Regex source generator for a HEX string.
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^(0x|0X)?[a-fA-F0-9]+$", 
            RegexOptions.CultureInvariant, 1000)]
        public static partial Regex HexRegex();

        /// <summary>
        /// Regex source generator for the special template text. Used for creating custom passwords via <see cref="PasswordRule"/>.<see cref="PasswordRule.New"/>.
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex("{special:?\\d*}")]
        public static partial Regex SpecialTemplateRegex();

        /// <summary>
        /// Regex source generator for the name template text. Used for creating custom passwords via <see cref="PasswordRule"/>.<see cref="PasswordRule.New"/>.
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex("{name=\"\\w*\\s*\\w+\";?(lowerFirst|upperFirst|lf|uf|lF|uF|Lf|Uf)?,(lowerSecond|upperSecond|ls|us|lS|uS|Ls|Us)?}")]
        public static partial Regex NameTemplateRegex();

        /// <summary>
        /// Regex source generator for the date template text. Used for creating custom passwords via <see cref="PasswordRule"/>.<see cref="PasswordRule.New"/>.
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex("{date}")]
        public static partial Regex DateTemplateRegex();
    }
}
