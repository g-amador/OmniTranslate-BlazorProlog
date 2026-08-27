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

        // Maps 6‑dot binary Braille patterns (from Prolog) to Unicode Braille characters.
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
            ["001111"] = "⠼" // Number sign (⠼) = binary 001111
        };

        /// <summary>
        /// Unique identifier for this translator.
        /// </summary>
        public string Id => "english_to_braille";

        /// <summary>
        /// Source language.
        /// </summary>
        public string From => "English";

        /// <summary>
        /// Target language.
        /// </summary>
        public string To => "Braille";

        /// <summary>
        /// Label used in the UI dropdown.
        /// </summary>
        public string Label => "English <-> Braille";

        /// <summary>
        /// Initializes the Prolog engine and loads the Braille dictionary.
        /// </summary>
        public EnglishToBrailleTranslator()
        {
            _engine = new PrologEngine(false);
            _engine.ConsultFromString(File.ReadAllText("Prolog/braille.pl"));
        }

        /// <summary>
        /// Translates English text into Unicode Braille.
        /// </summary>
        public string Translate(string input)
        {
            return TranslateEnglishToBraille(input);
        }

        // Converts English text into Unicode Braille.
        // - Letters → Unicode Braille
        // - Digits → number sign + Unicode Braille
        // - Words separated by 3 spaces
        private string TranslateEnglishToBraille(string input)
        {
            var sb = new StringBuilder();
            bool inNumberMode = false; // tracks digit sequences

            foreach (char ch in input.ToLower())
            {
                // Word separator
                if (ch == ' ')
                {
                    if (sb.Length > 0 && sb[^1] == ' ')
                    {
                        sb.Length--; // remove trailing space
                    }

                    sb.Append("   "); // 3-space word separator
                    inNumberMode = false;
                    continue;
                }

                // Add number sign once per digit sequence
                if (char.IsDigit(ch) && !inNumberMode)
                {
                    sb.Append("⠼ ");
                    inNumberMode = true;
                }

                // Ask Prolog for the binary Braille pattern
                string? binary = QueryBrailleBinary(ch);

                // Convert binary → Unicode Braille
                string unicode = binary is null
                    ? ch.ToString() // fallback for unknown characters
                    : BinaryToUnicode.TryGetValue(binary, out string? value)
                        ? value
                        : ch.ToString(); // fallback for unknown characters

                sb.Append(unicode);
                sb.Append(' ');
            }

            // Remove trailing space
            if (sb.Length > 0 && sb[^1] == ' ')
            {
                sb.Length--;
            }

            return sb.ToString();
        }

        // Queries Prolog for the binary Braille pattern of a character.
        private string? QueryBrailleBinary(char ch)
        {
            var query = $"braille(\"{ch}\", B).";
            var sol = _engine.GetFirstSolution(query);

            // If Prolog has no mapping, return the character itself
            if (!sol.Solved)
            {
                return ch.ToString();
            }

            // sol.ToString() looks like: braille("a","100000").
            var fact = sol.ToString();
            var parts = fact!.Split('"');

            return parts.Length >= 2 ? parts[1] : null;
        }
    }
}
