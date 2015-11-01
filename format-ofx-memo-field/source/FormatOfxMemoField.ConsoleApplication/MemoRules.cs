using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace FormatOfxMemoField.ConsoleApplication
{
    public class MemoRules
    {
        public List<string> RemoveFromStartOfMemos { get; set; }
        public List<string> Locations { get; set; }
        public List<string> CompanyNames { get; set; }
        public List<MemoReplacementRule> Replacements { get; set; }

        public static MemoRules Parse(string memoRulesFileName)
        {
            if (!File.Exists(memoRulesFileName))
            {
                throw new Exception($"Cannot proceed because '{memoRulesFileName}' cannot be found.");
            }

            var deserializer = new Deserializer();
            var yaml = File.ReadAllText(memoRulesFileName);

            using (var reader = new StringReader(yaml))
            {
                var result = deserializer.Deserialize<MemoRules>(reader);
                return result;
            }
        }
    }
}