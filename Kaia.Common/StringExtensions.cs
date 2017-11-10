using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kaia.Common
{
    public static class StringExtensions
    {
        private static IDictionary<string, string> _exceptions = 
            new Dictionary<string, string>
        {
            { "man", "men" },
            { "woman", "women" },
            { "child","children" },
            { "mouse", "mice" },
            { "sheep", "sheep" },
            { "goose", "geese" },
            { "data", "data" },
            { "radius", "radii" }
        };

        private static IList<string> _esEndings = new string[]
        {
            "s", "x", "sh", "tch"
        };

        public static string ToSingular(this string s)
        {
            var exception = 
                _exceptions.FirstOrDefault(e => e.Value == s.ToLowerInvariant());
            if (!string.IsNullOrEmpty(exception.Value))
            {
                return exception.Key;
            }
            else if (_esEndings.Select(e => string.Concat(e, "es"))
                .Any(e => s.EndsWith(e)))
            {
                return s.Substring(0, s.Length - 2);
            }
            else if (s.EndsWith("ies"))
            {
                return string.Concat(s.Substring(0, s.Length - 3), "y");
            }
            else if (s.EndsWith("s"))
            {
                return s.Substring(0, s.Length - 1);
            }
            return s;
        }

        public static string ToPlural(this string s)
        {
            if (_exceptions.ContainsKey(s.ToLowerInvariant()))
            {
                return _exceptions[s.ToLowerInvariant()];
            }
            else if (_esEndings.Any(e => s.EndsWith(e, 
                StringComparison.InvariantCultureIgnoreCase)))
            {
                var ending = _esEndings.First(e => s.EndsWith(e, 
                    StringComparison.InvariantCultureIgnoreCase));
                return string.Concat(s.Substring(0, s.Length - ending.Length), "es");
            }
            else if (s.EndsWith("y") && !s.EndsWith("ey"))
            {
                return string.Concat(s.Substring(0, s.Length - 1), "ies");
            }
            return string.Concat(s, "s");
        }

        private static IList<string> SplitIdentifier(string identifier)
        {
            var rxCase = new Regex("[a-z][A-Z]");

            if (identifier.IndexOf('-') >= 0)
            {
                return identifier.Split(new char[] { '-' });
            }
            else if (identifier.IndexOf('_') >= 0)
            {
                return identifier.Split(new char[] { '_' });
            }
            else if (rxCase.IsMatch(identifier))
            {
                var matches = rxCase.Matches(identifier);
                var start = 0;
                var result = new List<string>();
                foreach (Match match in matches)
                {
                    result.Add(identifier.Substring(start, match.Index - start + 1));
                    start = match.Index + 1;
                }
                result.Add(identifier.Substring(start));
                return result;
            }
            return new string[] { identifier };
        }


        public static string ToInitialCap(this string s)
        {
            return string.IsNullOrEmpty(s) ? s : 
                string.Concat(s.Substring(0, 1).ToUpperInvariant(), 
                    s.Substring(1).ToLowerInvariant());
        }


        public static string ToSnakeCaseLower(this string s)
        {
            return string.IsNullOrEmpty(s) ? s :
                string.Join("_", SplitIdentifier(s)).ToLowerInvariant();
        }


        public static string ToSnakeCaseUpper(this string s)
        {
            return string.IsNullOrEmpty(s) ? s :
                string.Join("_", SplitIdentifier(s)).ToUpperInvariant();
        }


        public static string ToKebabCaseLower(this string s)
        {
            return string.IsNullOrEmpty(s) ? s :
                string.Join("-", SplitIdentifier(s)).ToLowerInvariant();
        }


        public static string ToKebabCaseUpper(this string s)
        {
            return string.IsNullOrEmpty(s) ? s :
                string.Join("-", SplitIdentifier(s)).ToUpperInvariant();
        }


        public static string ToCamelCase(this string s)
        {
            return string.Concat(SplitIdentifier(s)
                .Select((v, i) => i == 0 ? v.ToLowerInvariant() : v.ToInitialCap()));
        }


        public static string ToPascalCase(this string s)
        {
            return string.Concat(SplitIdentifier(s)
                .Select(v => v.ToInitialCap()));
        }
    }
}
