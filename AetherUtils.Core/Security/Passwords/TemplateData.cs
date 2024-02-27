using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AetherUtils.Core.RegEx;
using AetherUtils.Core.Utility;

namespace AetherUtils.Core.Security.Passwords;

internal sealed class TemplateData
{
    private readonly Dictionary<Regex, (string, int)> _templateExpressions;
    private readonly List<int>  _replaceIndices = [];
    private string _templateText { get; set; }
    
    /// <summary>
    /// Get or set the special characters allowed in a password.
    /// </summary>
    internal char[] SpecialChars { get; set; } = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~".ToCharArray();

    /// <summary>
    /// Get or set the integers allowed in a password.
    /// </summary>
    internal char[] NumberChars { get; set; } = ['1','2','3','4','5','6','7','8','9','0'];
    
    internal TemplateData(string templateText)
    {
        _templateText = templateText;
        
        _templateExpressions = new Dictionary<Regex, (string, int)>
        {
            {
                RegexGenerator.SpecialTemplateRegex(), ("[s]", 0)
            },
            {
                RegexGenerator.NameTemplateRegex(), ("[n]", 0)
            },
            {
                RegexGenerator.DateTemplateRegex(), ("[d]", 0)
            },
        };
    }

    internal string GetTemplatePassword()
    {
        ReplaceTemplateText();
        FindReplacementIndices();
        
        var nameText = ParseName();
        var specials = GetSpecials(_templateExpressions[RegexGenerator.SpecialTemplateRegex()].Item2);
        var date = ParseDate().Replace(",", "");
        var final = ParsedTemplate;
        
        final = final.Replace(_templateExpressions[RegexGenerator.NameTemplateRegex()].Item1, nameText);
        final = final.Replace(_templateExpressions[RegexGenerator.DateTemplateRegex()].Item1, date);

        var specialTemplateString = _templateExpressions[RegexGenerator.SpecialTemplateRegex()].Item1;
        
        //add specials
        while (final.Contains(specialTemplateString))
        {
            var index = final.IndexOf(specialTemplateString, StringComparison.Ordinal);
            if (index == -1)
                continue;
            
            final = final.Insert(index, $"{specials[RandomNumberGenerator.GetInt32(0, specials.Length)]}");
            final = final.Remove(index + 1, specialTemplateString.Length);
        }
        return final;
    }

    /// <summary>
    /// Replaces the template text with their shorter equivalents based on the list of template expressions.
    /// <remarks>This method modifies <see cref="ParsedTemplate"/> and sets it to a new string with all occurrences
    /// of the template expressions replaced with their shorter variants.</remarks>
    /// </summary>
    private void ReplaceTemplateText()
    {
        var builder = new StringBuilder(ParsedTemplate);
        
        foreach (var template in _templateExpressions)
        {
            var matches = template.Key.Matches(ParsedTemplate);
            if (matches.Count <= 0)
                continue;
            
            foreach (Match m in matches)
            {
                builder = builder.Replace(m.Value, _templateExpressions[template.Key].Item1);
                var tuple = _templateExpressions[template.Key];
                tuple.Item2++;
                _templateExpressions[template.Key] = (tuple.Item1, tuple.Item2);
            }
        }
        ParsedTemplate = builder.ToString();
    }
    
    /// <summary>
    /// Finds in the template string the actual indices that need to be replaced with their template value and adds those indices to the
    /// <see cref="_replaceIndices"/> list.
    /// <remarks>This method should be called after <see cref="ReplaceTemplateText"/>
    /// and <see cref="ParsedTemplate"/> has been replaced with the shorthand variant.</remarks>
    /// </summary>
    private void FindReplacementIndices()
    {
        var count = 0;
        while (count < ParsedTemplate.Length)
        {
            foreach (var index in 
                     _templateExpressions.Values
                         .Select(tuple => ParsedTemplate
                             .IndexOf(tuple.Item1, count, StringComparison.Ordinal))
                         .Where(index => index > 0))
            {
                _replaceIndices.Add(index);
            }
            count++;
        }
    }

    private string ParseName()
    {
        StringBuilder bldr = new();
        var options = string.Empty;
        
        foreach (Match match in RegexGenerator.NameTemplateRegex().Matches(_templateText))
        {
            bldr = new StringBuilder();
            
            var nameStr = _templateText.Substring(match.Index, match.Length);
            options = string.Empty;

            if (nameStr.Contains(';'))
            {
                options = nameStr[nameStr.IndexOf(';')..nameStr.IndexOf('}')]; //parse out the options if they exist
                nameStr = nameStr.Remove(nameStr.IndexOf(';')); //remove extra options from name string
            }

            nameStr = nameStr.Replace("{name=\"", ""); //remove extra information.
            bldr = bldr.Append(nameStr);
            
            //Modify name string based on modifies (if they exist)
            var lastNameIndex = -1;
            if (options.Equals(string.Empty))
                continue;
            
            var tokens = options.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in tokens)
            {
                var t = token.Replace(";", "").Replace(",", ""); //remove extra characters from token, if they exist.
                bldr[0] = t switch
                {
                    "upperFirst" => char.Parse(bldr[0].ToString().ToUpper()),
                    "uF" => char.Parse(bldr[0].ToString().ToUpper()),
                    "uf" => char.Parse(bldr[0].ToString().ToUpper()),
                    "Uf" => char.Parse(bldr[0].ToString().ToUpper()),
                    "UF" => char.Parse(bldr[0].ToString().ToUpper()),
                    "lowerFirst" => char.Parse(bldr[0].ToString().ToLower()),
                    "lF" => char.Parse(bldr[0].ToString().ToLower()),
                    "lf" => char.Parse(bldr[0].ToString().ToLower()),
                    "Lf" => char.Parse(bldr[0].ToString().ToLower()),
                    "LF" => char.Parse(bldr[0].ToString().ToLower()),
                    _ => bldr[0]
                };

                lastNameIndex = bldr.ToString().IndexOf(' ') + 1;
                if (lastNameIndex > 0)
                {
                    bldr[lastNameIndex] = t switch
                    {
                        "upperSecond" => char.Parse(bldr[lastNameIndex].ToString().ToUpper()),
                        "uS" => char.Parse(bldr[lastNameIndex].ToString().ToUpper()),
                        "us" => char.Parse(bldr[lastNameIndex].ToString().ToUpper()),
                        "Us" => char.Parse(bldr[lastNameIndex].ToString().ToUpper()),
                        "US" => char.Parse(bldr[lastNameIndex].ToString().ToUpper()),
                        "lowerSecond" => char.Parse(bldr[lastNameIndex].ToString().ToLower()),
                        "lS" => char.Parse(bldr[lastNameIndex].ToString().ToLower()),
                        "ls" => char.Parse(bldr[lastNameIndex].ToString().ToLower()),
                        "Ls" => char.Parse(bldr[lastNameIndex].ToString().ToLower()),
                        "LS" => char.Parse(bldr[lastNameIndex].ToString().ToLower()),
                        _ => bldr[0]
                    };
                }
            }
        }
        //return firstname character and lastname character or just the firstname character if no last name
        return $"{bldr[0]}{bldr[bldr.ToString().IndexOf(' ') + 1]}";
    }

    private string ParseDate()
    {
        StringBuilder bldr = new();
        foreach (Match match in RegexGenerator.DateTemplateRegex().Matches(_templateText))
            bldr = bldr.Append(DateTime.Now.Year).Append(',');
        return bldr.ToString();
    }

    /// <summary>
    /// Get the specified count of cryptographically random special characters based on the special characters allowed for a password.
    /// </summary>
    /// <param name="count">The number of characters to select from the specials list.</param>
    /// <returns>A character array containing the special characters.</returns>
    private char[] GetSpecials(int count = 2)
    {
        List<char> specials = [];

        for (var i = 0; i < count; i++)
            specials.Add(RuleData.SpecialChars[RandomNumberGenerator.GetInt32(
                RuleData.SpecialChars.Length)]);
        return specials.ToArray();
    }
}