using OmniTranslate_BlazorProlog.Services.Interfaces;
using Prolog;
using System.Text;

namespace OmniTranslate_BlazorProlog.Services.Implementations.Translators
{
    /// <summary>
    /// Translator that converts Minion language text into English using Prolog rules.
    /// Supports multi-word Minion expressions such as "po ka" and "bee do bee do".
    /// Preserves punctuation and natural spacing.
    /// </summary>
    public class MinionToEnglishTranslator : ITranslator
    {
        private readonly PrologEngine _engine;

        /// <summary>
        /// Unique identifier for this translator.
        /// </summary>
        public string Id => "minion_to_english";

        /// <summary>
        /// Source language name.
        /// </summary>
        public string From => "Minion";

        /// <summary>
        /// Target language name.
        /// </summary>
        public string To => "English";

        /// <summary>
        /// Human‑readable label used in the UI.
        /// </summary>
        public string Label => "English <-> Minion";

        /// <summary>
        /// Initializes the Prolog engine and loads the Minion dictionary.
        /// </summary>
        public MinionToEnglishTranslator()
        {
            _engine = new PrologEngine(false);
            _engine.ConsultFromString(File.ReadAllText("Prolog/minion.pl"));
        }

        /// <summary>
        /// Translates a Minion-language sentence into English.
        /// Handles multi-word Minion expressions and preserves punctuation.
        /// </summary>
        public string Translate(string input)
        {
            // Tokenize the input into words and punctuation.
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
                    {
                        sb.Append(' ');
                    }

                    i++;
                    continue;
                }

                string translated = null;

                // Try matching 4-word Minion expressions (e.g., "bee do bee do").
                if (i + 3 < tokens.Count)
                {
                    var four = $"{tokens[i]} {tokens[i + 1]} {tokens[i + 2]} {tokens[i + 3]}";
                    translated = QueryMinion(four);

                    if (translated != null)
                    {
                        sb.Append(translated);
                        AddSpaceIfNeeded(tokens, sb, i + 4);
                        i += 4;
                        continue;
                    }
                }

                // Try matching 3-word expressions.
                if (i + 2 < tokens.Count)
                {
                    var three = $"{tokens[i]} {tokens[i + 1]} {tokens[i + 2]}";
                    translated = QueryMinion(three);

                    if (translated != null)
                    {
                        sb.Append(translated);
                        AddSpaceIfNeeded(tokens, sb, i + 3);
                        i += 3;
                        continue;
                    }
                }

                // Try matching 2-word expressions (e.g., "po ka").
                if (i + 1 < tokens.Count)
                {
                    var two = $"{tokens[i]} {tokens[i + 1]}";
                    translated = QueryMinion(two);

                    if (translated != null)
                    {
                        sb.Append(translated);
                        AddSpaceIfNeeded(tokens, sb, i + 2);
                        i += 2;
                        continue;
                    }
                }

                // Fallback: try translating a single Minion word.
                translated = QueryMinion(tokens[i]) ?? tokens[i];

                sb.Append(translated);
                AddSpaceIfNeeded(tokens, sb, i + 1);
                i++;
            }

            return sb.ToString().Trim();
        }

        private string QueryMinion(string minion)
        {
            // Build a Prolog query to translate the Minion phrase.
            var query = $"minion_word(E, \"{minion}\").";

            // Execute the query and retrieve the first matching solution.
            var sol = _engine.GetFirstSolution(query);

            if (!sol.Solved)
            {
                return null;
            }

            // sol.ToString() returns something like: minion_word("hello","bello").
            var fact = sol.ToString();
            var parts = fact.Split('"');

            // Extract the English translation from the Prolog fact.
            return parts.Length >= 2 ? parts[1] : null;
        }

        private static List<string> Tokenize(string input)
        {
            var tokens = new List<string>();
            var current = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetterOrDigit(c))
                {
                    // Build up a word token.
                    current.Append(c);
                }
                else
                {
                    // Emit the current word before handling punctuation.
                    if (current.Length > 0)
                    {
                        tokens.Add(current.ToString());
                        current.Clear();
                    }

                    // Add punctuation as its own token (ignore whitespace).
                    if (!char.IsWhiteSpace(c))
                    {
                        tokens.Add(c.ToString());
                    }
                }
            }

            // Emit any leftover word.
            if (current.Length > 0)
            {
                tokens.Add(current.ToString());
            }

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
            {
                sb.Append(' ');
            }
        }
    }
}
