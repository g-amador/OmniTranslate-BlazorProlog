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
        /// <param name="input">The Minion text to translate.</param>
        /// <returns>The translated English text.</returns>
        public string Translate(string input)
        {
            var tokens = Tokenize(input.ToLower());
            var sb = new StringBuilder();

            int i = 0;
            while (i < tokens.Count)
            {
                // If token is punctuation, append it and add a space only if next token is a word.
                if (IsPunctuation(tokens[i]))
                {
                    sb.Append(tokens[i]);

                    if (i + 1 < tokens.Count && char.IsLetter(tokens[i + 1][0]))
                        sb.Append(' ');

                    i++;
                    continue;
                }

                string translated = null;

                // Try 4-word Minion expressions (e.g., "bee do bee do")
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

                // Try 3-word expressions
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

                // Try 2-word expressions (e.g., "po ka", "tank tu")
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

                // Try 1-word match
                translated = QueryMinion(tokens[i]) ?? tokens[i];
                sb.Append(translated);
                AddSpaceIfNeeded(tokens, sb, i + 1);
                i++;
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Queries Prolog for the English translation of a Minion word or phrase.
        /// </summary>
        private string QueryMinion(string minion)
        {
            var query = $"minion_word(E, \"{minion}\").";
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

            foreach (char c in input)
            {
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
