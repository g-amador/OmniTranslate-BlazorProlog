using OmniTranslate_BlazorProlog.Services.Interfaces;
using Prolog;
using System.Text;
using System.Text.RegularExpressions;

namespace OmniTranslate_BlazorProlog.Services.Implementations.Translators
{
    /// <summary>
    /// Translates Morse code into English text using Prolog rules
    /// defined in <c>morse.pl</c>. Uses string-based Prolog facts
    /// for maximum compatibility and Unicode safety.
    /// </summary>
    public class MorseToEnglishTranslator : ITranslator
    {
        private readonly PrologEngine _engine;

        /// <summary>
        /// Unique identifier for this translator.
        /// </summary>
        public string Id => "morse_to_english";

        /// <summary>
        /// Source language or encoding name.
        /// </summary>
        public string From => "Morse";

        /// <summary>
        /// Target language or encoding name.
        /// </summary>
        public string To => "English";

        /// <summary>
        /// Human‑readable label used in the UI.
        /// </summary>
        public string Label => "English <-> Morse";

        /// <summary>
        /// Initializes the translator and loads the Morse dictionary
        /// from the Prolog file <c>morse.pl</c>.
        /// </summary>
        public MorseToEnglishTranslator()
        {
            _engine = new PrologEngine(false);
            _engine.ConsultFromString(File.ReadAllText("Prolog/morse.pl"));
        }

        /// <summary>
        /// Translates Morse code into English text.
        /// </summary>
        public string Translate(string input)
        {
            return TranslateMorseToEnglish(input);
        }

        private string TranslateMorseToEnglish(string input)
        {
            var sb = new StringBuilder();

            // Split the input into words.
            // In Morse, words are separated by 3 or more spaces.
            var words = Regex.Split(input, @"\s{3,}");

            for (int w = 0; w < words.Length; w++)
            {
                // Split each word into Morse letters.
                // Letters are separated by exactly 1 space.
                var letters = Regex.Split(words[w], @"(?<=\S) (?=\S)");

                foreach (var morse in letters)
                {
                    // Ask Prolog for the English character for this Morse sequence.
                    // If no mapping exists, keep the Morse sequence unchanged.
                    string translated = QueryMorse(morse) ?? morse;

                    sb.Append(translated);
                }

                // Add a space between English words.
                if (w < words.Length - 1)
                {
                    sb.Append(' ');
                }
            }

            return sb.ToString();
        }

        private string? QueryMorse(string morse)
        {
            // Build a Prolog query to retrieve the English character.
            var query = $"code(X, \"{morse}\").";

            // Execute the query and get the first matching solution.
            var sol = _engine.GetFirstSolution(query);

            if (!sol.Solved)
            {
                return null;
            }

            // sol.ToString() returns something like: code("a",".-").
            var fact = sol.ToString();
            var parts = fact!.Split('"');

            // Extract the English character from the Prolog fact.
            return parts.Length >= 2 ? parts[1] : null;
        }
    }
}
