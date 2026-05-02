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

        public string Id => "morse_to_english";
        public string From => "Morse";
        public string To => "English";

        public MorseToEnglishTranslator()
        {
            _engine = new PrologEngine(false);
            _engine.ConsultFromString(File.ReadAllText("Prolog/morse.pl"));
        }

        public string Translate(string input)
        {
            return TranslateMorseToEnglish(input);
        }

        /// <summary>
        /// Converts Morse code into English text.
        /// Words are separated by 3+ spaces, letters by exactly 1 space.
        /// </summary>
        private string TranslateMorseToEnglish(string input)
        {
            var sb = new StringBuilder();

            // STEP 1: split words by 3 or more spaces
            var words = Regex.Split(input, @"\s{3,}");

            for (int w = 0; w < words.Length; w++)
            {
                // STEP 2: split letters by EXACTLY 1 space
                var letters = Regex.Split(words[w], @"(?<=\S) (?=\S)");

                foreach (var morse in letters)
                {
                    string translated = QueryMorse(morse) ?? morse;
                    sb.Append(translated);
                }

                if (w < words.Length - 1)
                    sb.Append(' ');
            }

            return sb.ToString();
        }

        /// <summary>
        /// Queries Prolog for the English character represented by a Morse sequence.
        /// Uses string-based Prolog facts: code("a", ".-").
        /// </summary>
        private string QueryMorse(string morse)
        {
            var query = $"code(X, \"{morse}\").";
            var sol = _engine.GetFirstSolution(query);

            if (!sol.Solved)
                return null;

            // sol.ToString() returns: code("a",".-").
            var fact = sol.ToString();
            var parts = fact.Split('"');

            // parts[1] = English letter
            return parts.Length >= 2 ? parts[1] : null;
        }
    }
}
