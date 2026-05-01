using OmniTranslate_BlazorProlog.Services.Interfaces;
using Prolog;
using System.Text;

namespace OmniTranslate_BlazorProlog.Services.Implementations
{
    /// <summary>
    /// Translator that converts Morse code into English text
    /// using Prolog rules defined in <c>morse.pl</c>.
    /// </summary>
    public class MorseToEnglishTranslator : ITranslator
    {
        private readonly PrologEngine _engine;

        /// <summary>
        /// Characters supported by this translator.
        /// Used when checking possible Prolog matches.
        /// </summary>
        private readonly char[] Letters =
            "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        /// <inheritdoc />
        public string Id => "morse_to_english";

        /// <inheritdoc />
        public string From => "Morse";

        /// <inheritdoc />
        public string To => "English";

        /// <summary>
        /// Initializes a new instance of the translator and loads
        /// the Prolog rules for Morse code translation.
        /// </summary>
        public MorseToEnglishTranslator()
        {
            _engine = new PrologEngine(false);
            _engine.ConsultFromString(File.ReadAllText("Prolog/morse.pl"));
        }

        /// <summary>
        /// Translates Morse code into English text.
        /// </summary>
        /// <param name="input">The Morse code string to translate.</param>
        /// <returns>The English representation of the input.</returns>
        public string Translate(string input)
        {
            return TranslateMorseToEnglish(input);
        }

        /// <summary>
        /// Performs the actual Morse → English translation using Prolog lookups.
        /// </summary>
        private string TranslateMorseToEnglish(string input)
        {
            var sb = new StringBuilder();

            // STEP 1: split words by 3 spaces (standard Morse word separator)
            var words = input.Split("   ", StringSplitOptions.None);

            for (int w = 0; w < words.Length; w++)
            {
                string result = "?";

                // STEP 2: split letters by 1 space
                var letters = words[w].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var morse in letters)
                {
                    // Try to match the Morse sequence to a known character
                    foreach (var ch in Letters)
                    {
                        var query = $"code({ch}, \"{morse}\").";
                        var sol = _engine.GetFirstSolution(query);

                        if (sol.Solved)
                        {
                            result = ch.ToString();
                            break;
                        }
                    }

                    // If no match was found, preserve the raw Morse sequence
                    if (result == "?")
                    {
                        result = morse;
                    }

                    sb.Append(result);
                }

                // STEP 3: add space between words (but not after the last)
                if (w < words.Length - 1)
                    sb.Append(' ');
            }

            return sb.ToString();
        }
    }
}
