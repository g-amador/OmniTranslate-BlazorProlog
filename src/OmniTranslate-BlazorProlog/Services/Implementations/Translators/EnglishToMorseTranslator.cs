using OmniTranslate_BlazorProlog.Services.Interfaces;
using Prolog;
using System.Text;

namespace OmniTranslate_BlazorProlog.Services.Implementations.Translators
{
    /// <summary>
    /// Translates English text into Morse code using Prolog rules
    /// defined in <c>morse.pl</c>. Uses string-based Prolog facts
    /// for maximum compatibility and Unicode safety.
    /// </summary>
    public class EnglishToMorseTranslator : ITranslator
    {
        private readonly PrologEngine _engine;

        /// <summary>
        /// Unique identifier for this translator.
        /// </summary>
        public string Id => "english_to_morse";

        /// <summary>
        /// Source language or encoding name.
        /// </summary>
        public string From => "English";

        /// <summary>
        /// Target language or encoding name.
        /// </summary>
        public string To => "Morse";

        /// <summary>
        /// Human‑readable label used in the UI.
        /// </summary>
        public string Label => "English <-> Morse";

        /// <summary>
        /// Initializes the translator and loads the Morse dictionary
        /// from the Prolog file <c>morse.pl</c>.
        /// </summary>
        public EnglishToMorseTranslator()
        {
            _engine = new PrologEngine(false);
            _engine.ConsultFromString(File.ReadAllText("Prolog/morse.pl"));
        }

        /// <summary>
        /// Translates English text into Morse code.
        /// </summary>
        public string Translate(string input)
        {
            return TranslateEnglishToMorse(input);
        }

        private string TranslateEnglishToMorse(string input)
        {
            var sb = new StringBuilder();

            foreach (char ch in input.ToLower())
            {
                // When encountering a space, we insert a 3‑space separator
                // which represents a word boundary in Morse code.
                if (ch == ' ')
                {
                    // Avoid creating accidental double spaces before inserting the separator.
                    if (sb.Length > 0 && sb[^1] == ' ')
                    {
                        sb.Length--;
                    }

                    sb.Append("   "); // Morse word separator
                    continue;
                }

                // Ask Prolog for the Morse representation of this character.
                // If no mapping exists, fall back to the original character.
                string result = QueryMorse(ch) ?? ch.ToString();

                // Append the Morse code followed by a space (letter separator).
                sb.Append(result);
                sb.Append(' ');
            }

            // Remove trailing space at the end of the output.
            if (sb.Length > 0 && sb[^1] == ' ')
            {
                sb.Length--;
            }

            return sb.ToString();
        }

        private string QueryMorse(char ch)
        {
            // Convert the character into a Prolog atom.
            string atom = ch.ToString();

            // Build a Prolog query to retrieve the Morse code for this character.
            var query = $"code(\"{atom}\", M).";

            // Execute the query and get the first matching solution.
            var sol = _engine.GetFirstSolution(query);

            if (!sol.Solved)
            {
                return null;
            }

            // sol.ToString() returns something like: code("a",".-").
            var fact = sol.ToString();
            var parts = fact.Split('"');

            // Extract the Morse code from the Prolog fact.
            return parts.Length >= 2 ? parts[1] : null;
        }
    }
}
