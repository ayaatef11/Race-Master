using System.Text.RegularExpressions;

namespace RunGroopWebApp.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
            };

        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower(); // removes diacritical marks (accents) from the characters in the phrase.
                                                          // Accents are common in characters like "é", "ü", or "ç".         
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");  
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-");   
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);//The code page determines how characters are mapped to bytes.
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
