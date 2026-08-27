using OmniTranslate_BlazorProlog.Services.Interfaces;
using Prolog;
using System.Text;

namespace OmniTranslate_BlazorProlog.Services.Implementations.Translators
{
    /// <summary>
    /// Translates English text into Minion language using Prolog rules.
    /// Handles punctuation and preserves spacing.
    /// </summary>
    public class EnglishToMinionTranslator : ITranslator
    {
        private readonly PrologEngine _engine;

        /// <inheritdoc />
        public string Id => "english_to_minion";

        /// <inheritdoc />
        public string From => "English";

        /// <inheritdoc />
        public string To => "Minion";

        /// <summary>
        /// Human‑readable label used in the UI.
        /// </summary>
        public string Label => "English <-> Minion";

        /// <summary>
        /// Initializes the translator and loads the Minion dictionary
        /// from the Prolog file <c>minion.pl</c>.
        /// </summary>
        public EnglishToMinionTranslator()
        {
            _engine = new PrologEngine(false);
            _engine.ConsultFromString(File.ReadAllText("Prolog/minion.pl"));
        }

        /// <summary>
        /// Translates English text into Minion.
        /// </summary>
        public string Translate(string input)
        {
            return TranslateEnglishToMinion(input);
        }

        private static string Clean(string word)
        {
            // Remove any non-letter characters from the word.
            // This is used to isolate the actual word before translation.
            return new string(word.Where(char.IsLetter).ToArray());
        }

        private string TranslateEnglishToMinion(string input)
        {
            var sb = new StringBuilder();

            // First split the input into words based on spaces.
            // Each word may still contain punctuation.
            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var raw in words)
            {
                // Split each word into smaller tokens:
                // - alphabetic sequences (words)
                // - punctuation marks (.,!? etc.)
                foreach (var token in SplitWordAndPunctuation(raw))
                {
                    if (char.IsLetter(token[0]))
                    {
                        // Build a Prolog query to translate the English word.
                        var query = $"minion_word(\"{token.ToLower()}\", M).";
                        var sol = _engine.GetFirstSolution(query);

                        if (sol.Solved)
                        {
                            // sol.ToString() returns something like: minion_word("hello","bello").
                            // Extract the Minion translation from the Prolog fact.
                            var fact = sol.ToString();
                            var result = fact!.Split('"')[1];
                            sb.Append(result);
                        }
                        else
                        {
                            // If no translation exists, keep the original word.
                            sb.Append(token);
                        }
                    }
                    else
                    {
                        // If the token is punctuation, append it unchanged.
                        sb.Append(token);
                    }
                }

                // Add a space after each processed word group.
                sb.Append(' ');
            }

            // Trim trailing space for clean output.
            return sb.ToString().Trim();
        }

        private static IEnumerable<string> SplitWordAndPunctuation(string token)
        {
            var current = new StringBuilder();

            foreach (char c in token)
            {
                if (char.IsLetter(c))
                {
                    // Build up alphabetic sequences into a word token.
                    current.Append(c);
                }
                else
                {
                    // When punctuation is encountered, emit the current word (if any).
                    if (current.Length > 0)
                    {
                        yield return current.ToString();
                        current.Clear();
                    }

                    // Emit punctuation as its own token.
                    yield return c.ToString();
                }
            }

            // Emit any remaining word characters.
            if (current.Length > 0)
            {
                yield return current.ToString();
            }
        }
    }
}
