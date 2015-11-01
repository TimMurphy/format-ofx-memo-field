using System;
using System.IO;
using System.Threading.Tasks;

namespace FormatOfxMemoField.ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var arguments = Arguments.Parse(args);

                ExecuteAsync(
                    arguments.Source,
                    arguments.Output,
                    arguments.OverwriteOutput,
                    arguments.MemoRules).Wait();
            }
            catch (AggregateException exception)
            {
                WriteException(exception.InnerExceptions.Count == 1 ? exception.InnerException : exception);
            }
            catch (Exception exception)
            {
                WriteException(exception);
            }
        }

        public static Task ExecuteAsync(string sourceFileName, string outputFileName, bool overwriteOutputFile, string memoRulesFileName)
        {
            return Task.Run(() => Execute(sourceFileName, outputFileName, overwriteOutputFile, memoRulesFileName));
        }

        public static void Execute(string sourceFileName, string outputFileName, bool overwriteOutputFile, string memoRulesFileName)
        {
            var memoRules = MemoRules.Parse(memoRulesFileName);
            var memoFormatter = new MemoFormatter(memoRules);

            Execute(sourceFileName, outputFileName, overwriteOutputFile, memoFormatter);
        }

        public static void Execute(string sourceFileName, string outputFileName, bool overwriteOutputFile, MemoFormatter memoFormatter)
        {
            if (!File.Exists(sourceFileName))
            {
                throw new InvalidOperationException($"Cannot proceed because '{sourceFileName}' does not exist.");
            }

            if (!overwriteOutputFile && File.Exists(outputFileName))
            {
                throw new InvalidOperationException($"Cannot proceed because '{outputFileName}' exists and overwrite is false.");
            }

            Console.WriteLine($"Reading {sourceFileName}...");
            var lines = File.ReadAllLines(sourceFileName);

            FormatLines(lines, memoFormatter);
            memoFormatter.WriteSummary();

            Console.WriteLine($"Writing {outputFileName}...");
            File.WriteAllLines(outputFileName, lines);

            Console.WriteLine($"Successfully formatted {sourceFileName}.");
        }

        private static void WriteException(Exception exception)
        {
            var foregroundColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception.Message);
            Console.WriteLine();
            Console.WriteLine(exception);
            Console.ForegroundColor = foregroundColor;
        }

        private static void FormatLines(string[] lines, MemoFormatter memoFormatter)
        {
            for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                var originalLine = lines[lineNumber];
                var formattedLine = FormatLine(originalLine, memoFormatter);
                lines[lineNumber] = formattedLine;
            }
        }

        private static string FormatLine(string originalLine, MemoFormatter memoFormatter)
        {
            return originalLine.StartsWith("<MEMO>") ? FormatMemoLine(originalLine, memoFormatter) : originalLine;
        }

        private static string FormatMemoLine(string originalMemoLine, MemoFormatter memoFormatter)
        {
            return $"<MEMO>{memoFormatter.Format(GetMemoValue(originalMemoLine))}";
        }

        private static string GetMemoValue(string originalMemoLine)
        {
            // Get the text after <MEMO>
            return originalMemoLine.Substring(6);
        }
    }
}