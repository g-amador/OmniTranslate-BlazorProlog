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
        /// <param name="input">The English text to translate.</param>
        /// <returns>The translated Minion text.</returns>
        public string Translate(string input)
        {
            return TranslateEnglishToMinion(input);
        }

        /// <summary>
        /// Removes punctuation from a word, leaving only letters.
        /// </summary>
        private static string Clean(string word)
        {
            return new string(word.Where(char.IsLetter).ToArray());
        }

        /// <summary>
        /// Performs the actual English → Minion translation.
        /// Splits words and punctuation, queries Prolog for translations,
        /// and preserves original punctuation and spacing.
        /// </summary>
        private string TranslateEnglishToMinion(string input)
        {
            var sb = new StringBuilder();

            // Split by spaces first (word-level)
            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var raw in words)
            {
                // Further split each word into [word][punctuation] tokens
                foreach (var token in SplitWordAndPunctuation(raw))
                {
                    if (char.IsLetter(token[0]))
                    {
                        // Query Prolog for translation
                        var query = $"minion_word(\"{token.ToLower()}\", M).";
                        var sol = _engine.GetFirstSolution(query);

                        if (sol.Solved)
                        {
                            // sol.ToString() returns something like: minion_word("hello","bello").
                            var fact = sol.ToString();
                            var result = fact.Split('"')[1]; // Extract Minion word
                            sb.Append(result);
                        }
                        else
                        {
                            // No translation found → keep original word
                            sb.Append(token);
                        }
                    }
                    else
                    {
                        // Token is punctuation → append as-is
                        sb.Append(token);
                    }
                }

                // Add space after each processed word group
                sb.Append(' ');
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Splits a token into separate word and punctuation parts.
        /// Example: "hello," → ["hello", ","]
        /// </summary>
        private static IEnumerable<string> SplitWordAndPunctuation(string token)
        {
            var current = new StringBuilder();

            foreach (char c in token)
            {
                if (char.IsLetter(c))
                {
                    current.Append(c);
                }
                else
                {
                    // Emit accumulated word before punctuation
                    if (current.Length > 0)
                    {
                        yield return current.ToString();
                        current.Clear();
                    }

                    // Emit punctuation as its own token
                    yield return c.ToString();
                }
            }

            // Emit final word if any
            if (current.Length > 0)
                yield return current.ToString();
        }
    }
}
