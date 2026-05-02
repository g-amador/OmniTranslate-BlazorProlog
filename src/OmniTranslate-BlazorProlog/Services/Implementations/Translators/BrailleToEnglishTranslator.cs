using OmniTranslate_BlazorProlog.Services.Interfaces;
using Prolog;
using System.Text;
using System.Text.RegularExpressions;

namespace OmniTranslate_BlazorProlog.Services.Implementations.Translators
{
    /// <summary>
    /// Translates Unicode Braille symbols into English text.
    /// Input uses Unicode Braille (⠁ ⠃ ⠉ …), but braille.pl uses binary patterns.
    /// This translator converts Unicode → binary → English using Prolog.
    /// </summary>
    public class BrailleToEnglishTranslator : ITranslator
    {
        private readonly PrologEngine _engine;

        /// <summary>
        /// Maps Unicode Braille characters to their 6‑dot binary patterns.
        /// This allows us to keep braille.pl unchanged (binary-based)
        /// while accepting Unicode Braille as input.
        /// </summary>
        private static readonly Dictionary<string, string> UnicodeToBinary = new()
        {
            ["⠁"] = "100000",
            ["⠃"] = "110000",
            ["⠉"] = "100100",
            ["⠙"] = "100110",
            ["⠑"] = "100010",
            ["⠋"] = "110100",
            ["⠛"] = "110110",
            ["⠓"] = "110010",
            ["⠊"] = "010100",
            ["⠚"] = "010110",
            ["⠅"] = "101000",
            ["⠇"] = "111000",
            ["⠍"] = "101100",
            ["⠝"] = "101110",
            ["⠕"] = "101010",
            ["⠏"] = "111100",
            ["⠟"] = "111110",
            ["⠗"] = "111010",
            ["⠎"] = "011100",
            ["⠞"] = "011110",
            ["⠥"] = "101001",
            ["⠧"] = "111001",
            ["⠺"] = "010111",
            ["⠭"] = "101101",
            ["⠽"] = "101111",
            ["⠵"] = "101011",

            // Number sign (⠼) = binary 001111
            ["⠼"] = "001111"
        };

        public string Id => "braille_to_english";
        public string From => "Braille";
        public string To => "English";

        public BrailleToEnglishTranslator()
        {
            // Load Prolog dictionary (binary-based)
            _engine = new PrologEngine(false);
            _engine.ConsultFromString(File.ReadAllText("Prolog/braille.pl"));
        }

        public string Translate(string input)
        {
            return TranslateBrailleToEnglish(input);
        }

        /// <summary>
        /// Converts Unicode Braille into English text.
        /// - Unicode symbols → binary
        /// - binary → English via Prolog
        /// - Handles number mode (⠼)
        /// - Words separated by 3 spaces
        /// </summary>
        private string TranslateBrailleToEnglish(string input)
        {
            var sb = new StringBuilder();
            bool inNumberMode = false;

            // Split words by 3+ spaces (Braille word separator)
            var words = Regex.Split(input, @"\s{3,}");

            for (int w = 0; w < words.Length; w++)
            {
                // Split Unicode Braille symbols by 1 space
                var symbols = words[w].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var symbol in symbols)
                {
                    // NUMBER SIGN (⠼) — activates number mode
                    if (symbol == "⠼")
                    {
                        inNumberMode = true;
                        continue;
                    }

                    // Convert Unicode → binary pattern
                    if (!UnicodeToBinary.TryGetValue(symbol, out var binary))
                    {
                        // Unknown symbol — output as-is
                        sb.Append(symbol);
                        continue;
                    }

                    // Query Prolog to get the English character for this binary pattern
                    string raw = QueryBraille(binary);

                    if (raw == null)
                    {
                        sb.Append(symbol);
                        continue;
                    }

                    // If raw is NOT a–j, number mode ends BEFORE conversion
                    if (inNumberMode && !"abcdefghij".Contains(raw))
                        inNumberMode = false;

                    string translated = raw;

                    // Convert a–j → 1–0 ONLY when number mode is active
                    if (inNumberMode)
                    {
                        translated = raw switch
                        {
                            "a" => "1",
                            "b" => "2",
                            "c" => "3",
                            "d" => "4",
                            "e" => "5",
                            "f" => "6",
                            "g" => "7",
                            "h" => "8",
                            "i" => "9",
                            "j" => "0",
                            _ => raw
                        };
                    }

                    sb.Append(translated);
                }

                // Add a space between words
                if (w < words.Length - 1)
                {
                    sb.Append(' ');
                    inNumberMode = false; // number mode always ends at word boundary
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Queries Prolog for the English character corresponding to a binary Braille pattern.
        /// Example fact: braille("a", "100000").
        /// </summary>
        private string QueryBraille(string binaryPattern)
        {
            var query = $"braille(X, \"{binaryPattern}\").";
            var sol = _engine.GetFirstSolution(query);

            if (!sol.Solved)
                return null;

            // sol.ToString() returns: braille("a","100000").
            var fact = sol.ToString();
            var parts = fact.Split('"');

            // parts[1] = English letter/digit
            return parts.Length >= 2 ? parts[1] : null;
        }
    }
}
