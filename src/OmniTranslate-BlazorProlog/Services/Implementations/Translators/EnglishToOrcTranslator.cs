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

        /// <summary>
        /// Extracts the first quoted string from a Prolog solution.
        /// Works for both formats:
        ///   orc_word("hi","charach").
        ///   O = "charach"
        /// </summary>
        private static string ExtractQuotedValue(string fact)
        {
            var parts = fact.Split('"');
            return parts.Length >= 2 ? parts[1] : fact;
        }

        /// <summary>
        /// Performs the actual English → Orc translation.
        /// Splits words and punctuation, queries Prolog for translations,
        /// and preserves original punctuation and spacing.
        /// </summary>
        private string TranslateEnglishToOrc(string input)
        {
            var sb = new StringBuilder();
            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var raw in words)
            {
                foreach (var token in SplitWordAndPunctuation(raw))
                {
                    if (char.IsLetter(token[0]))
                    {
                        var query = $"orc_word(\"{token.ToLower()}\", O).";
                        var sol = _engine.GetFirstSolution(query);

                        if (sol.Solved)
                        {
                            var fact = sol.ToString();
                            var result = ExtractQuotedValue(fact);
                            sb.Append(result);
                        }
                        else
                        {
                            sb.Append(token);
                        }
                    }
                    else
                    {
                        sb.Append(token);
                    }
                }

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
                    if (current.Length > 0)
                    {
                        yield return current.ToString();
                        current.Clear();
                    }

                    yield return c.ToString();
                }
            }

            if (current.Length > 0)
                yield return current.ToString();
        }
    }
}
