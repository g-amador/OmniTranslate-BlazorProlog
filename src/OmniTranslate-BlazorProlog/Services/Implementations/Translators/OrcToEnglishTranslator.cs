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
        /// Human‑readable label used in the UI.
        /// </summary>
        public string Label => "English <-> Orc";

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
            // Break the input into tokens (words, punctuation, line breaks).
            var tokens = Tokenize(input.ToLower());
            var sb = new StringBuilder();

            int i = 0;

            while (i < tokens.Count)
            {
                // If the token is punctuation, append it directly.
                if (IsPunctuation(tokens[i]))
                {
                    sb.Append(tokens[i]);

                    // Add a space after punctuation only if the next token is a word.
                    if (i + 1 < tokens.Count && char.IsLetter(tokens[i + 1][0]))
                        sb.Append(' ');

                    i++;
                    continue;
                }

                // Orc has only 1‑word entries, but we keep the structure consistent
                // with MinionToEnglish for future extensibility.
                string translated = QueryOrc(tokens[i]) ?? tokens[i];

                sb.Append(translated);
                AddSpaceIfNeeded(tokens, sb, i + 1);
                i++;
            }

            return sb.ToString().Trim();
        }

        private string QueryOrc(string orc)
        {
            // Build a Prolog query to translate the Orc word.
            var query = $"orc_word(E, \"{orc}\").";

            // Execute the query and retrieve the first matching solution.
            var sol = _engine.GetFirstSolution(query);

            if (!sol.Solved)
                return null;

            // sol.ToString() returns something like: orc_word("hi","charach").
            var fact = sol.ToString();
            var parts = fact.Split('"');

            // Extract the English translation from the Prolog fact.
            return parts.Length >= 2 ? parts[1] : null;
        }

        private static List<string> Tokenize(string input)
        {
            var tokens = new List<string>();
            var current = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (char.IsLetterOrDigit(c))
                {
                    // Build up a word token.
                    current.Append(c);
                }
                else
                {
                    // Emit the current word before handling punctuation or line breaks.
                    if (current.Length > 0)
                    {
                        tokens.Add(current.ToString());
                        current.Clear();
                    }

                    // Preserve CRLF sequences as their own tokens.
                    if (c == '\r' && i + 1 < input.Length && input[i + 1] == '\n')
                    {
                        tokens.Add("\r\n");
                        i++; // Skip the '\n'
                        continue;
                    }

                    // Preserve LF as its own token.
                    if (c == '\n')
                    {
                        tokens.Add("\n");
                        continue;
                    }

                    // Add punctuation as its own token (ignore whitespace).
                    if (!char.IsWhiteSpace(c))
                        tokens.Add(c.ToString());
                }
            }

            // Emit any leftover word.
            if (current.Length > 0)
                tokens.Add(current.ToString());

            return tokens;
        }

        private static bool IsPunctuation(string token)
        {
            // A punctuation token is a single non-letter character.
            return token.Length == 1 && char.IsPunctuation(token[0]);
        }

        private static void AddSpaceIfNeeded(List<string> tokens, StringBuilder sb, int nextIndex)
        {
            // Add a space only if the next token is a word (not punctuation).
            if (nextIndex < tokens.Count && !IsPunctuation(tokens[nextIndex]))
                sb.Append(' ');
        }
    }
}
