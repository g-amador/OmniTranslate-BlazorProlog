using OmniTranslate_BlazorProlog.Services.Interfaces;
using Prolog;
using System.Text;

namespace OmniTranslate_BlazorProlog.Services.Implementations.Translators
{
    /// <summary>
    /// Translates English text into Unicode Braille symbols.
    /// Uses Prolog to map English characters to 6‑dot binary patterns,
    /// then converts those binary patterns into Unicode Braille.
    /// </summary>
    public class EnglishToBrailleTranslator : ITranslator
    {
        private readonly PrologEngine _engine;

        /// <summary>
        /// Maps 6‑dot binary Braille patterns (from Prolog) to Unicode Braille characters.
        /// This allows us to keep braille.pl unchanged (binary-based)
        /// while still outputting readable Unicode Braille.
        /// </summary>
        private static readonly Dictionary<string, string> BinaryToUnicode = new()
        {
            ["100000"] = "⠁",
            ["110000"] = "⠃",
            ["100100"] = "⠉",
            ["100110"] = "⠙",
            ["100010"] = "⠑",
            ["110100"] = "⠋",
            ["110110"] = "⠛",
            ["110010"] = "⠓",
            ["010100"] = "⠊",
            ["010110"] = "⠚",
            ["101000"] = "⠅",
            ["111000"] = "⠇",
            ["101100"] = "⠍",
            ["101110"] = "⠝",
            ["101010"] = "⠕",
            ["111100"] = "⠏",
            ["111110"] = "⠟",
            ["111010"] = "⠗",
            ["011100"] = "⠎",
            ["011110"] = "⠞",
            ["101001"] = "⠥",
            ["111001"] = "⠧",
            ["010111"] = "⠺",
            ["101101"] = "⠭",
            ["101111"] = "⠽",
            ["101011"] = "⠵",

            // Number sign (⠼) = binary 001111
            ["001111"] = "⠼"
        };

        public string Id => "english_to_braille";
        public string From => "English";
        public string To => "Braille";

        public EnglishToBrailleTranslator()
        {
            // Load Prolog dictionary (binary-based)
            _engine = new PrologEngine(false);
            _engine.ConsultFromString(File.ReadAllText("Prolog/braille.pl"));
        }

        public string Translate(string input)
        {
            return TranslateEnglishToBraille(input);
        }

        /// <summary>
        /// Converts English text into Unicode Braille.
        /// - Letters → Unicode Braille
        /// - Digits → number sign + Unicode Braille
        /// - Words separated by 3 spaces
        /// </summary>
        private string TranslateEnglishToBraille(string input)
        {
            var sb = new StringBuilder();
            bool inNumberMode = false; // Tracks whether we are inside a digit sequence

            foreach (char ch in input.ToLower())
            {
                // WORD SEPARATOR
                if (ch == ' ')
                {
                    // Remove trailing space before adding 3-space separator
                    if (sb.Length > 0 && sb[^1] == ' ')
                        sb.Length--;

                    sb.Append("   "); // 3 spaces between words
                    inNumberMode = false; // Number mode ends at word boundary
                    continue;
                }

                // NUMBER SIGN (⠼) — only added once per digit sequence
                if (char.IsDigit(ch) && !inNumberMode)
                {
                    sb.Append("⠼ ");
                    inNumberMode = true;
                }

                // Query Prolog to get the binary Braille pattern for this character
                string binary = QueryBrailleBinary(ch);

                // Convert binary → Unicode Braille
                string unicode = BinaryToUnicode.ContainsKey(binary)
                    ? BinaryToUnicode[binary]
                    : ch.ToString(); // fallback for unknown characters

                // Append the Unicode Braille symbol
                sb.Append(unicode);
                sb.Append(' ');
            }

            // Remove trailing space
            if (sb.Length > 0 && sb[^1] == ' ')
                sb.Length--;

            return sb.ToString();
        }

        /// <summary>
        /// Queries Prolog for the binary Braille pattern of a character.
        /// Example Prolog fact: braille("a", "100000").
        /// </summary>
        private string QueryBrailleBinary(char ch)
        {
            var query = $"braille(\"{ch}\", B).";
            var sol = _engine.GetFirstSolution(query);

            // If Prolog has no mapping, return the character itself
            if (!sol.Solved)
                return ch.ToString();

            // sol.ToString() looks like: braille("a","100000").
            var fact = sol.ToString();
            var parts = fact.Split('"');

            // parts[1] = binary Braille pattern
            return parts.Length >= 2 ? parts[1] : null;
        }
    }
}
