using OmniTranslate_BlazorProlog.Models;

namespace OmniTranslate_BlazorProlog.Services
{
    /// <summary>
    /// Provides all available translation modes for the UI dropdown.
    /// Each mode represents a translation direction (e.g., English → Orc).
    /// The UI uses these modes to determine which translator to invoke.
    /// </summary>
    public class TranslationModeProvider
    {
        /// <summary>
        /// List of all supported translation modes.
        /// Each mode contains:
        /// - Id: unique translator identifier
        /// - Label: text shown in the dropdown
        /// - From: source language
        /// - To: target language
        /// </summary>
        public List<TranslationMode> Modes { get; }

        /// <summary>
        /// The default translation mode used when the app loads.
        /// </summary>
        public TranslationMode Default => Modes.First(m => m.Id == "english_to_morse");

        /// <summary>
        /// Builds the list of translation modes by reading all registered translators
        /// from the <see cref="TranslationRegistry"/>.
        /// </summary>
        /// <param name="registry">Registry containing all translator instances.</param>
        public TranslationModeProvider(TranslationRegistry registry)
        {
            // Convert each translator into a TranslationMode object.
            // This keeps UI concerns separate from translator implementation details.
            Modes = registry.Translators.Values
                .Select(t => new TranslationMode
                {
                    Id = t.Id,       // Unique translator ID (e.g., "english_to_orc")
                    Label = t.Label, // Display label (e.g., "English → Orc")
                    From = t.From,   // Source language
                    To = t.To        // Target language
                })
                .ToList();
        }
    }
}
