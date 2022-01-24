using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Findle
{
    public class CommandLineOptions
    {
        [Value(index: 0, Required = true, HelpText = "Word file")]
        public string File { get; set; }

        [Option(shortName: 'e', Required = false, HelpText = "Letters elimintated from consideration.")]
        public string Eliminated { get; set; } = string.Empty;

        [Option(shortName: 'a', Required = false, HelpText = "Letters that are anchors for the word. Use '.' as placeholders.")]
        public string Anchors { get; set; } = string.Empty;

        [Option(shortName: 'r', Required = false, HelpText = "Letters that are required for the word.")]
        public string Required { get; set; } = string.Empty;
    }

    class Program
    {
        private static string[] StarterWords = {
            "ARISE",
            "ROAST",
            "TEARS",
            "MEATS",
            "OUIJA",
            "PIZZA",
            "AUDIO",
            "FARTS"
        };

        static void Main(
            string[] args
        )
        {
            bool isSuccess = Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult((CommandLineOptions opts) =>
                {
                    try
                    {
                        // We have the parsed arguments, so let's just pass them down
                        IEnumerable<string> potentialWords = FindWords(opts).OrderBy(w => w);

                        foreach (string option in potentialWords)
                        {
                            Console.WriteLine(option.ToUpperInvariant());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing the command line! EX={ex}");
                    }

                    return true;
                },
                errs => false); // Invalid arguments

            if (!isSuccess)
            {
                Console.WriteLine("Sorry, you are error.");
            }
        }

        private static IEnumerable<string> GetWords(
            string file
        )
        {
            IEnumerable<string> words = File.ReadLines(file);

            IEnumerable<string> validWords = words
                .Select(w => w?.Trim() ?? string.Empty)
                .Select(w => w.ToLowerInvariant())
                .Where(w => w.Length == 5)
                .Where(w => !w.Any(c => !char.IsLetter(c)))
                .Distinct();

            return validWords;
        }

        private static IEnumerable<string> FindWords(
            in CommandLineOptions options
        )
        {
            string anchors = options.Anchors?.Trim().ToLowerInvariant().Replace("\"", "") ?? string.Empty;
            char[] requiredLetters = (options.Required?.Trim().ToLowerInvariant().Replace("\"", "") ?? string.Empty).ToCharArray();
            char[] eliminatedLetters = (options.Eliminated?.Trim().ToLowerInvariant().Replace("\"", "") ?? string.Empty).ToCharArray();

            if ((options == null)
                || (string.IsNullOrEmpty(anchors) && requiredLetters.Length == 0 && eliminatedLetters.Length == 0))
            {
                return StarterWords;
            }

            IEnumerable<string> words = GetWords(options.File);

            // Step 1
            //
            // Filter for words that have required letters
            // in required positions
            if (anchors.Length == 5)
            {
                Regex match = new(anchors, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                words = words.Where(w => match.IsMatch(w));
            }

            // Step 2
            //
            // Filter for words that have required letters.
            foreach (char required in requiredLetters)
            {
                words = words.Where(w => w.Contains(required));
            }

            // Step 3
            //
            // Filter out words that have letters that are fully eliminated
            foreach (char eliminated in eliminatedLetters)
            {
                words = words.Where(w => !w.Contains(eliminated));
            }

            return words;
        }
    }
}
