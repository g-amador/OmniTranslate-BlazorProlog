using OmniTranslate_BlazorProlog.Services.Interfaces;
using Prolog;
using System.Text;

namespace OmniTranslate_BlazorProlog.Services.Implementations
{
    /// <summary>
    /// Translator that converts English text into Morse code
    /// using Prolog rules defined in <c>morse.pl</c>.
    /// </summary>
    public class EnglishToMorseTranslator : ITranslator
    {
        private readonly PrologEngine _engine;

        /// <summary>
        /// Characters supported by this translator.
        /// Only these characters will be looked up in Prolog.
        /// </summary>
        private readonly char[] Letters =
            "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        /// <inheritdoc />
        public string Id => "english_to_morse";

        /// <inheritdoc />
        public string From => "English";

        /// <inheritdoc />
        public string To => "Morse";

        /// <summary>
        /// Initializes a new instance of the translator and loads
        /// the Prolog rules for Morse code translation.
        /// </summary>
        public EnglishToMorseTranslator()
        {
            _engine = new PrologEngine(false);
            _engine.ConsultFromString(File.ReadAllText("Prolog/morse.pl"));
        }

        /// <summary>
        /// Translates English text into Morse code.
        /// </summary>
        /// <param name="input">The English text to translate.</param>
        /// <returns>The Morse code representation of the input.</returns>
        public string Translate(string input)
        {
            return TranslateEnglishToMorse(input);
        }

        /// <summary>
        /// Performs the actual English → Morse translation using Prolog lookups.
        /// </summary>
        private string TranslateEnglishToMorse(string input)
        {
            var sb = new StringBuilder();

            foreach (var ch in input.ToLower())
            {
                // Preserve word boundaries using standard Morse spacing
                if (ch == ' ')
                {
                    sb.Append("   "); // 3 spaces = word separator
                    continue;
                }

                string result = ch.ToString();

                // Only attempt lookup for supported characters
                foreach (var letter in Letters)
                {
                    if (letter != ch)
                        continue;

                    // Query Prolog for the Morse representation
                    var query = $"code({letter}, M).";
                    var sol = _engine.GetFirstSolution(query);

                    if (sol.Solved)
                    {
                        // sol.ToString() returns something like: code(a, ".-").
                        var fact = sol.ToString();
                        var morse = fact.Split('"')[1];
                        result = morse;
                        break;
                    }
                }

                // Append Morse symbol followed by a space (letter separator)
                sb.Append(result + " ");
            }

            // Trim trailing space for clean output
            return sb.ToString().Trim();
        }
    }
}
