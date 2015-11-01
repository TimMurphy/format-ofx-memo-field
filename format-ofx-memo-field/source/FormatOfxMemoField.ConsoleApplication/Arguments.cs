using System;
using System.Linq;

namespace FormatOfxMemoField.ConsoleApplication
{
    public class Arguments
    {
        public Arguments(string source, string output, bool overwriteOutput, string memoRules)
        {
            Source = source;
            Output = output;
            OverwriteOutput = overwriteOutput;
            MemoRules = memoRules;
        }

        public string Source { get; }
        public string Output { get; }
        public bool OverwriteOutput { get; }
        public string MemoRules { get; }

        public static Arguments Parse(string[] args)
        {
            if (args.Any())
            {
                throw new NotImplementedException();
            }

            return new Arguments(@"C:\Users\Tim\Downloads\Data.ofx", @"C:\Users\Tim\Downloads\ynab.ofx", true, "MemoRules.Sample.yaml");
        }
    }
}