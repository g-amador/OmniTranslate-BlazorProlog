using OmniTranslate_BlazorProlog.Services.Interfaces;
using Prolog;
using System.Text;

namespace OmniTranslate_BlazorProlog.Services.Implementations.Translators
{
    /// <summary>
    /// Translator that converts Orc language text into English using Prolog rules.
    /// Preserves punctuation and natural spacing.
    /// </summary>
    public class OrcToEnglishTranslator : ITranslator
    {
        private readonly PrologEngine _engine;

        /// <summary>
        /// Unique identifier for this translator.
        /// </summary>
        public string Id => "orc_to_english";

        /// <summary>
        /// Source language name.
        /// </summary>
        public string From => "Orc";

        /// <summary>
        /// Target language name.
        /// </summary>
        public string To => "English";

        /// <summary>
        /// Initializes the Prolog engine and loads the Orc dictionary.
        /// </summary>
        public OrcToEnglishTranslator()
        {
            _engine = new PrologEngine(false);
            _engine.ConsultFromString(File.ReadAllText("Prolog/orc.pl"));
        }

        /// <summary>
        /// Translates an Orc-language sentence into English.
        /// Preserves punctuation and spacing.
        /// </summary>
        public string Translate(string input)
        {
            var tokens = Tokenize(input.ToLower());
            var sb = new StringBuilder();

            int i = 0;
            while (i < tokens.Count)
            {
                // Punctuation → append directly
                if (IsPunctuation(tokens[i]))
                {
                    sb.Append(tokens[i]);

                    if (i + 1 < tokens.Count && char.IsLetter(tokens[i + 1][0]))
                        sb.Append(' ');

                    i++;
                    continue;
                }

                // Orc has only 1-word entries, but we keep the structure for consistency
                string translated = QueryOrc(tokens[i]) ?? tokens[i];

                sb.Append(translated);
                AddSpaceIfNeeded(tokens, sb, i + 1);
                i++;
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Queries Prolog for the English translation of an Orc word.
        /// </summary>
        private string QueryOrc(string orc)
        {
            var query = $"orc_word(E, \"{orc}\").";
            var sol = _engine.GetFirstSolution(query);

            if (!sol.Solved)
                return null;

            var fact = sol.ToString();
            var parts = fact.Split('"');

            return parts.Length >= 2 ? parts[1] : null;
        }

        /// <summary>
        /// Splits text into tokens: words and punctuation as separate items.
        /// </summary>
        private static List<string> Tokenize(string input)
        {
            var tokens = new List<string>();
            var current = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (char.IsLetterOrDigit(c))
                {
                    current.Append(c);
                }
                else
                {
                    if (current.Length > 0)
                    {
                        tokens.Add(current.ToString());
                        current.Clear();
                    }

                    // Preserve CRLF (\r\n)
                    if (c == '\r' && i + 1 < input.Length && input[i + 1] == '\n')
                    {
                        tokens.Add("\r\n");
                        i++; // skip \n
                        continue;
                    }

                    // Preserve LF (\n)
                    if (c == '\n')
                    {
                        tokens.Add("\n");
                        continue;
                    }

                    // Preserve punctuation
                    if (!char.IsWhiteSpace(c))
                        tokens.Add(c.ToString());
                }
            }

            if (current.Length > 0)
                tokens.Add(current.ToString());

            return tokens;
        }

        /// <summary>
        /// Determines whether a token is punctuation.
        /// </summary>
        private static bool IsPunctuation(string token)
        {
            return token.Length == 1 && char.IsPunctuation(token[0]);
        }

        /// <summary>
        /// Adds a space after a translated word if the next token is another word.
        /// Prevents spaces before punctuation.
        /// </summary>
        private static void AddSpaceIfNeeded(List<string> tokens, StringBuilder sb, int nextIndex)
        {
            if (nextIndex < tokens.Count && !IsPunctuation(tokens[nextIndex]))
                sb.Append(' ');
        }
    }
}
