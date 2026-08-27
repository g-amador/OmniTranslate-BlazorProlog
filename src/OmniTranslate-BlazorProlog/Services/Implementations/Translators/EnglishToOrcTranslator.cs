using OmniTranslate_BlazorProlog.Services.Interfaces;
using Prolog;
using System.Text;

namespace OmniTranslate_BlazorProlog.Services.Implementations.Translators
{
    /// <summary>
    /// Translates English text into Orc language using Prolog rules.
    /// Handles punctuation and preserves spacing.
    /// </summary>
    public class EnglishToOrcTranslator : ITranslator
    {
        private readonly PrologEngine _engine;

        /// <inheritdoc />
        public string Id => "english_to_orc";

        /// <inheritdoc />
        public string From => "English";

        /// <inheritdoc />
        public string To => "Orc";

        /// <summary>
        /// Human‑readable label used in the UI.
        /// </summary>
        public string Label => "English <-> Orc";

        /// <summary>
        /// Initializes the translator and loads the Orc dictionary
        /// from the Prolog file <c>orc.pl</c>.
        /// </summary>
        public EnglishToOrcTranslator()
        {
            _engine = new PrologEngine(false);
            _engine.ConsultFromString(File.ReadAllText("Prolog/orc.pl"));
        }

        /// <inheritdoc />
        public string Translate(string input)
        {
            return TranslateEnglishToOrc(input);
        }

        private static string ExtractQuotedValue(string fact)
        {
            // Prolog facts come in formats like:
            //   orc_word("hi","charach").
            //   O = "charach"
            // This extracts the first quoted value from the string.
            var parts = fact.Split('"');
            return parts.Length >= 2 ? parts[1] : fact;
        }

        private string TranslateEnglishToOrc(string input)
        {
            var sb = new StringBuilder();

            // Split the input into words based on spaces.
            // Each word may still contain punctuation.
            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var raw in words)
            {
                // Break each word into smaller tokens:
                // - alphabetic sequences (words)
                // - punctuation marks (.,!? etc.)
                foreach (var token in SplitWordAndPunctuation(raw))
                {
                    if (char.IsLetter(token[0]))
                    {
                        // Build a Prolog query to translate the English word.
                        var query = $"orc_word(\"{token.ToLower()}\", O).";
                        var sol = _engine.GetFirstSolution(query);

                        if (sol.Solved)
                        {
                            // Extract the Orc translation from the Prolog fact.
                            var fact = sol.ToString();
                            var result = ExtractQuotedValue(fact!);
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
                    // Emit the current word before handling punctuation.
                    if (current.Length > 0)
                    {
                        yield return current.ToString();
                        current.Clear();
                    }

                    // Emit punctuation as its own token.
                    yield return c.ToString();
                }
            }

            // Emit any leftover word.
            if (current.Length > 0)
            {
                yield return current.ToString();
            }
        }
    }
}
