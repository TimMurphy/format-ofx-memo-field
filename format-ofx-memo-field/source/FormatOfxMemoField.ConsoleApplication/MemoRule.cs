using System;
using FormatOfxMemoField.ConsoleApplication.Helpers;

namespace FormatOfxMemoField.ConsoleApplication
{
    internal class MemoRule
    {
        internal MemoRule(Func<string, bool> isRule, Func<string, string> replacement)
        {
            IsRule = isRule;
            Replacement = replacement;
        }

        internal Func<string, bool> IsRule { get; }
        internal Func<string, string> Replacement { get; }

        internal static MemoRule Contains(string searchFor, string replaceWith)
        {
            return Contains(searchFor, value => replaceWith);
        }

        internal static MemoRule Contains(string searchFor, Func<string, string> replaceWith)
        {
            Func<string, bool> isRule = value => value.Contains(searchFor, StringComparison.OrdinalIgnoreCase);

            return new MemoRule(isRule, replaceWith);
        }

        public static MemoRule StartsWith(string searchFor, Func<string, string> replaceWith)
        {
            Func<string, bool> isRule = value => value.StartsWith(searchFor, StringComparison.OrdinalIgnoreCase);

            return new MemoRule(isRule, replaceWith);
        }
    }
}