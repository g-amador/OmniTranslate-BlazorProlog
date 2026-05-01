using Prolog;
using System.Text;

namespace OmniTranslate_BlazorProlog.Services
{
    public class PrologTranslationService
    {
        private readonly PrologEngine _engine;

        // All characters we support
        private readonly char[] Letters =
            "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        public PrologTranslationService()
        {
            _engine = new PrologEngine(false);

            // Load the single combined Prolog file
            _engine.ConsultFromString(
                File.ReadAllText("Prolog/morse.pl"));
        }

        public Task<string> Translate(string mode, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Task.FromResult("");

            return mode switch
            {
                "morse_to_english" => Task.FromResult(TranslateMorseToEnglish(input)),
                "english_to_morse" => Task.FromResult(TranslateEnglishToMorse(input)),
                _ => Task.FromResult("")
            };
        }

        private string TranslateMorseToEnglish(string input)
        {
            var sb = new StringBuilder();

            // STEP 1: split words by 3 spaces
            var words = input.Split("   ", StringSplitOptions.None);

            for (int w = 0; w < words.Length; w++)
            {
                string result = "?";

                // STEP 2: split letters by 1 space
                var letters = words[w].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var morse in letters)
                {
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

                    if (result == "?")
                    {
                        result = morse;
                    }

                    sb.Append(result);
                }

                // STEP 3: add space between words (but not after last)
                if (w < words.Length - 1)
                    sb.Append(' ');
            }

            return sb.ToString();
        }

        private string TranslateEnglishToMorse(string input)
        {
            var sb = new StringBuilder();

            foreach (var ch in input.ToLower())
            {
                if (ch == ' ')
                {
                    sb.Append("   "); // Word spacing
                    continue;
                }

                string result = ch.ToString();

                foreach (var letter in Letters)
                {
                    if (letter != ch)
                        continue;

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

                sb.Append(result + " ");
            }

            return sb.ToString().Trim();
        }
    }
}
