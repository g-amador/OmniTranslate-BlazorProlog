using OmniTranslate_BlazorProlog.Services.Interfaces;

namespace OmniTranslate_BlazorProlog.Services.Implementations
{
    /// <summary>
    /// Provides translation functionality backed by Prolog-based translators.
    /// This service delegates translation work to registered <see cref="ITranslator"/> instances.
    /// </summary>
    public class PrologTranslationService : IPrologTranslationService
    {
        /// <summary>
        /// Registry containing all available translators.
        /// Each translator is identified by a unique mode ID.
        /// </summary>
        private readonly TranslationRegistry _registry = new();

        /// <summary>
        /// Translates the specified input text using the translator
        /// associated with the given <paramref name="mode"/> identifier.
        /// </summary>
        /// <param name="mode">
        /// The unique translation mode ID (e.g., "english_to_morse").
        /// </param>
        /// <param name="input">
        /// The text to translate.
        /// </param>
        /// <returns>
        /// A task resolving to the translated text, or an empty string if
        /// the input is empty or the mode is not recognized.
        /// </returns>
        public Task<string> Translate(string mode, string input)
        {
            // Ignore empty input
            if (string.IsNullOrWhiteSpace(input))
                return Task.FromResult("");

            // Look up the translator for the requested mode
            if (_registry.Translators.TryGetValue(mode, out var translator))
                return Task.FromResult(translator.Translate(input));

            // Unknown mode → return empty result
            return Task.FromResult("");
        }
    }
}
