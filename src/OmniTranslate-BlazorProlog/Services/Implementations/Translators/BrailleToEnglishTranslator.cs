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

        // Maps Unicode Braille characters to their 6‑dot binary patterns.
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
            ["⠼"] = "001111"// Number sign (⠼) = binary 001111
        };

        /// <summary>
        /// Unique identifier for this translator.
        /// </summary>
        public string Id => "braille_to_english";

        /// <summary>
        /// Source language.
        /// </summary>
        public string From => "Braille";

        /// <summary>
        /// Target language.
        /// </summary>
        public string To => "English";

        /// <summary>
        /// Label used in the UI dropdown.
        /// </summary>
        public string Label => "English <-> Braille";

        /// <summary>
        /// Initializes the Prolog engine and loads the Braille dictionary.
        /// </summary>
        public BrailleToEnglishTranslator()
        {
            _engine = new PrologEngine(false);
            _engine.ConsultFromString(File.ReadAllText("Prolog/braille.pl"));
        }

        /// <summary>
        /// Translates Unicode Braille into English text.
        /// </summary>
        public string Translate(string input)
        {
            return TranslateBrailleToEnglish(input);
        }

        // Converts Unicode Braille into English text.
        // - Unicode symbols → binary
        // - binary → English via Prolog
        // - Handles number mode (⠼)
        // - Words separated by 3 spaces
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
                    // Number sign activates number mode
                    if (symbol == "⠼")
                    {
                        inNumberMode = true;
                        continue;
                    }

                    // Convert Unicode → binary
                    if (!UnicodeToBinary.TryGetValue(symbol, out var binary))
                    {
                        sb.Append(symbol); // unknown symbol
                        continue;
                    }

                    // Query Prolog for English character
                    string raw = QueryBraille(binary);

                    if (raw == null)
                    {
                        sb.Append(symbol);
                        continue;
                    }

                    // If raw is NOT a–j, number mode ends
                    if (inNumberMode && !"abcdefghij".Contains(raw))
                        inNumberMode = false;

                    string translated = raw;

                    // Convert a–j → 1–0 when number mode is active
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

                // Add space between words
                if (w < words.Length - 1)
                {
                    sb.Append(' ');
                    inNumberMode = false; // number mode ends at word boundary
                }
            }

            return sb.ToString();
        }

        // Queries Prolog for the English character corresponding to a binary Braille pattern.
        private string QueryBraille(string binaryPattern)
        {
            var query = $"braille(X, \"{binaryPattern}\").";
            var sol = _engine.GetFirstSolution(query);

            if (!sol.Solved)
                return null;

            // sol.ToString() looks like: braille("a","100000").
            var fact = sol.ToString();
            var parts = fact.Split('"');

            return parts.Length >= 2 ? parts[1] : null;
        }
    }
}
