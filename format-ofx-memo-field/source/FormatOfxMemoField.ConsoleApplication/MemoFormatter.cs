using System;
using System.Collections.Generic;
using System.Linq;
using FormatOfxMemoField.ConsoleApplication.Helpers;
using Humanizer;

namespace FormatOfxMemoField.ConsoleApplication
{
    public class MemoFormatter
    {
        private readonly MemoRules _memoRules;
        private readonly List<string> _values = new List<string>();

        public MemoFormatter(MemoRules memoRules)
        {
            _memoRules = memoRules;
        }

        public string Format(string originalMemoValue)
        {
            var memoValue = originalMemoValue.Trim();

            memoValue = FormatReplacements(memoValue, _memoRules.Replacements);
            memoValue = RemoveSecondColumn(memoValue);
            memoValue = RemoveStartsWith(memoValue, _memoRules.RemoveFromStartOfMemos);
            memoValue = RemoveLocation(memoValue, _memoRules.Locations);
            memoValue = FormatCompany(memoValue, _memoRules.CompanyNames);
            memoValue = memoValue.ToLower().ApplyCase(LetterCasing.Title);
            memoValue = FormatCompany(memoValue, _memoRules.CompanyNames);

            _values.Add(memoValue);

            return memoValue;
        }

        public void WriteSummary()
        {
            Console.WriteLine("Memos");
            Console.WriteLine("-----");

            var groups = _values.GroupBy(value => value);

            foreach (var group in groups.OrderBy(grp => grp.Key))
            {
                Console.WriteLine($"{group.Key} ({group.Count()})");
            }
        }

        private static string FormatCompany(string memoValue, IEnumerable<string> companyNames)
        {
            var rules =
                from companyName in companyNames
                select MemoRule.Contains(companyName, companyName);

            return GetMemoValue(memoValue, rules);
        }

        private static string FormatReplacements(string memoValue, IEnumerable<MemoReplacementRule> replacementRules)
        {
            var rules =
                from replacementRule in replacementRules
                select MemoRule.Contains(replacementRule.SearchFor, replacementRule.ReplaceWith);

            return GetMemoValue(memoValue, rules);
        }

        private static string GetMemoValue(string memoValue, IEnumerable<MemoRule> rules)
        {
            var replacement = rules.FirstOrDefault(r => r.IsRule(memoValue));
            var result = replacement == null ? memoValue : replacement.Replacement(memoValue);

            return result;
        }

        private static string RemoveLocation(string memoValue, IEnumerable<string> locations)
        {
            var rules =
                from location in locations
                select MemoRule.Contains(location, value => value.TrimBefore(location));

            return GetMemoValue(memoValue, rules);
        }

        private static string RemoveSecondColumn(string memoValue)
        {
            var rules = new[] {MemoRule.Contains("  ", value => value.TrimBefore("  "))};

            return GetMemoValue(memoValue, rules);
        }

        private static string RemoveStartsWith(string memoValue, IEnumerable<string> startsWithValues)
        {
            var rules =
                from startsWithValue in startsWithValues
                select MemoRule.StartsWith(startsWithValue, value => value.TrimAfter(startsWithValue));

            return GetMemoValue(memoValue, rules);
        }
    }
}