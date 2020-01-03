using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Vnn88.Common.Infrastructure
{
    /// <summary>
    /// This class contain extension functions for string objects
    /// </summary>
    public static class StringExtensions
    {
        public enum TrimType
        {
            Comma,
            Pipe,
            Colon
        }

        /// <summary>
        /// Checks string object's value to array of string values
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stringValues">Array of string values to compare</param>
        /// <returns>Return true if any string value matches</returns>
        public static bool In(this string value, params string[] stringValues)
        {
            foreach (string otherValue in stringValues)
                if (String.CompareOrdinal(value, otherValue) == 0)
                    return true;

            return false;
        }

        /// <summary>
        /// Converts string to enum object
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">string value to convert</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Returns characters from right of specified length
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from right</returns>
        public static string Right(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(value.Length - length) : value;
        }

        /// <summary>
        /// Returns characters from left of specified length
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from left</returns>
        public static string Left(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(0, length) : value;
        }

        /// <summary>
        ///  Replaces the format item in a specified System.string with the text equivalent
        ///  of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="value">A composite format string</param>
        /// <param name="arg0">An System.Object to format</param>
        /// <returns>A copy of format in which the first format item has been replaced by the
        /// System.string equivalent of arg0</returns>
        public static string Format(this string value, object arg0)
        {
            return string.Format(value, arg0);
        }

        /// <summary>
        ///  Replaces the format item in a specified System.string with the text equivalent
        ///  of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="value">A composite format string</param>
        /// <param name="args">An System.Object array containing zero or more objects to format.</param>
        /// <returns>A copy of format in which the first format item has been replaced by the
        /// System.string equivalent of arg0</returns>
        public static string Format(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        /// <summary>
        ///  Truncate at word with specified length
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="length">Length of string will be truncated</param>
        /// <param name="bEclipse">Add eclipse at end of input string or not</param>
        /// <returns>Returns string</returns>
        public static string TruncateAtWord(this string value, int length, bool bEclipse)
        {
            if (value == null || value.Length < length)
                return value;
            int iNextSpace = value.LastIndexOf(" ", length, StringComparison.Ordinal);
            if (bEclipse)
                return string.Format("{0}...", value.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
            else
                return value.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim();
        }

        /// <summary>
        ///  Truncate at word with specified word count
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="wordCount">Number of word will be kept</param>
        /// <returns>Returns string</returns>
        public static string TruncateWithWordCount(string value, int wordCount)
        {
            if (!string.IsNullOrEmpty(value.Trim()))
            {
                return value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Take(wordCount).Aggregate((a, b) => a + " " + b);
            }
            return string.Empty;
        }

        /// <summary>
        ///  Trim any duplicate spaces between words
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>Returns string</returns>
        public static string TrimSpacesBetweenWords(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return Regex.Replace(value.Trim(), @"\s+", " ");
            }
            return string.Empty;
        }

        /// <summary>
        /// Counts number of words in a string
        /// </summary>
        /// <param name="str">The string to parse</param>
        /// <returns>An integer of the number of words found</returns>
        public static int WordCount(this string str)
        {
            return str.Split(new[] { ' ', '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        /// <summary>
        /// Uppercases the first character of a string
        /// </summary>
        /// <param name="input">The string which first character should be uppercased</param>
        /// <returns>The input string with it'input first character uppercased</returns>
        public static string FirstCharToUpper(this string input)
        {
            return string.IsNullOrEmpty(input) ? "" : string.Concat(input.Substring(0, 1).ToUpper(), input.Substring(1));
        }

        /// <summary>
        /// Highlights specified keywords in the input string with the specified class name by using a &lt;span /&gt;
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="keywords">The keywords to highlight</param>
        /// <param name="className">The class name</param>
        /// <returns>The input string with highlighted keywords</returns>
        public static string HighlightKeywords(this string input, IEnumerable<string> keywords, string className)
        {
            if (string.IsNullOrEmpty(input) || keywords == null || !keywords.Any())
                return input;

            foreach (string keyword in keywords)
            {
                input = Regex.Replace(input, keyword, string.Format("<span class=\"{1}\">{0}</span>", "$0", className), RegexOptions.IgnoreCase);
            }
            return input;
        }

        /// <summary>
        /// Removes diacritics from a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(string input)
        {
            // Indicates that a Unicode string is normalized using full canonical decomposition.
            string inputInFormD = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var item in inputInFormD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(item);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(item);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        /// <summary>
        /// Inverts the case of each character in the given string and returns the new string
        /// </summary>
        /// <param name="input">The given string</param>
        /// <returns>The converted string</returns>
        public static string InvertCase(this string input)
        {
            return new string(
            input.Select(c => char.IsLetter(c) ? (char.IsUpper(c) ?
                      char.ToLower(c) : char.ToUpper(c)) : c).ToArray());
        }

        /// <summary>
        /// Returns the substring of string1 before the occurrence of string2.
        /// </summary>
        /// <param name="string1">
        /// The string 1.
        /// </param>
        /// <param name="string2">
        /// The string 2.
        /// </param>
        /// <returns>
        /// The substring
        /// </returns>
        public static string SubstringBefore(this string string1, string string2)
        {
            var posA = string1.IndexOf(string2, StringComparison.Ordinal);
            if (posA != -1)
            {
                return string1.Substring(0, posA);
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns the substring of string1 after the occurrence of string2.
        /// </summary>
        /// <param name="string1">
        /// The string 1.
        /// </param>
        /// <param name="string2">
        /// The string 2.
        /// </param>
        /// <returns>
        /// The substring
        /// </returns>
        public static string SubstringAfter(this string string1, string string2)
        {
            var posA = string1.IndexOf(string2, StringComparison.Ordinal);
            if (posA != -1)
            {
                return string1.Substring(posA + 1);
            }
            return string.Empty;
        }

        public static bool ContainsAny(this string str, params string[] values)
        {
            if (!string.IsNullOrEmpty(str) || values.Length == 0)
            {
                foreach (string value in values)
                {
                    if (str != null && str.Contains(value))
                        return true;
                }
            }

            return false;
        }

        public static string DeleteChars(this string input, params char[] chars)
        {
            return new string(input.Where((ch) => !chars.Contains(ch)).ToArray());
        }

        public static string ReverseString(this string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string TrimDuplicates(this string input, TrimType trimType)
        {
            string result = string.Empty;

            switch (trimType)
            {
                case TrimType.Comma:
                    result = input.TrimCharacter(',');
                    break;
                case TrimType.Pipe:
                    result = input.TrimCharacter('|');
                    break;
                case TrimType.Colon:
                    result = input.TrimCharacter(':');
                    break;
            }

            return result;
        }

        private static string TrimCharacter(this string input, char character)
        {
            var result = string.Join(character.ToString(), input.Split(character)
                .Where(str => str != string.Empty)
                .ToArray());

            return result;
        }

        /// <summary>
        /// Formats the string according to the specified mask
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="mask">The mask for formatting. Like "A##-##-T-###Z"</param>
        /// <returns>The formatted string</returns>
        public static string FormatWithMask(this string input, string mask)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var output = string.Empty;
            var index = 0;
            foreach (var m in mask)
            {
                if (m == '#')
                {
                    if (index < input.Length)
                    {
                        output += input[index];
                        index++;
                    }
                }
                else
                    output += m;
            }
            return output;
        }

        public static bool IsNumeric(this string theValue)
        {
            return long.TryParse(theValue, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out long _);
        }

        public static string ToProperCase(this string text)
        {
            CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(text);
        }


        public static string TrimCommaBetweenWords(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return Regex.Replace(input.Trim(), ",", " ");
            }
            return string.Empty;
        }
    }
}