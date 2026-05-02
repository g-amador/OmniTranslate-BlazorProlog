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

        public string Id => "english_to_morse";
        public string From => "English";
        public string To => "Morse";

        public EnglishToMorseTranslator()
        {
            _engine = new PrologEngine(false);
            _engine.ConsultFromString(File.ReadAllText("Prolog/morse.pl"));
        }

        public string Translate(string input)
        {
            return TranslateEnglishToMorse(input);
        }

        /// <summary>
        /// Converts English text into Morse code.
        /// Letters separated by 1 space, words by 3 spaces.
        /// </summary>
        private string TranslateEnglishToMorse(string input)
        {
            var sb = new StringBuilder();

            foreach (char ch in input.ToLower())
            {
                if (ch == ' ')
                {
                    if (sb.Length > 0 && sb[^1] == ' ')
                        sb.Length--;

                    sb.Append("   ");
                    continue;
                }

                string result = QueryMorse(ch) ?? ch.ToString();

                sb.Append(result);
                sb.Append(' ');
            }

            if (sb.Length > 0 && sb[^1] == ' ')
                sb.Length--;

            return sb.ToString();
        }

        /// <summary>
        /// Queries Prolog for the Morse representation of a character.
        /// </summary>
        private string QueryMorse(char ch)
        {
            string atom = ch.ToString();
            var query = $"code(\"{atom}\", M).";

            var sol = _engine.GetFirstSolution(query);
            if (!sol.Solved)
                return null;

            var fact = sol.ToString();
            var parts = fact.Split('"');

            // parts[1] = Morse code
            return parts.Length >= 2 ? parts[1] : null;
        }
    }
}
