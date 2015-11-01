using System;

namespace FormatOfxMemoField.ConsoleApplication.Helpers
{
    internal static class StringExtensions
    {
        internal static bool Contains(this string value, string searchFor, StringComparison comparisonType)
        {
            var indexOf = value.IndexOf(searchFor, comparisonType);
            var result = indexOf > -1;

            return result;
        }

        internal static string TrimAfter(this string value, string trimAfter, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            var indexOf = value.IndexOf(trimAfter, comparisonType);

            return indexOf == -1 ? value : value.Substring(indexOf + trimAfter.Length).Trim();
        }

        internal static string TrimBefore(this string value, string trimBefore, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            var indexOf = value.IndexOf(trimBefore, comparisonType);

            return indexOf == -1 ? value : value.Substring(0, indexOf).Trim();
        }

        internal static string IfStartsWithThenReplaceWith(this string value, string startsWith, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            return value.IfStartsWithThenReplaceWith(startsWith, startsWith, comparisonType);
        }

        internal static string IfStartsWithThenReplaceWith(this string value, string startsWith, string replaceWith, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            return value.StartsWith(startsWith, comparisonType) ? replaceWith : value;
        }

        internal static string TrimStart(this string value, string trimString, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            return value.StartsWith(trimString, comparisonType) ? value.Substring(trimString.Length) : value;
        }
    }
}