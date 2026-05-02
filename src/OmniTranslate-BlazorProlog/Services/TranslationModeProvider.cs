using OmniTranslate_BlazorProlog.Models;

namespace OmniTranslate_BlazorProlog.Services
{
    /// <summary>
    /// Provides all available translation modes for the UI dropdown.
    /// Each mode defines a direction (English → Braille, Braille → English, etc.)
    /// and is used by the UI to determine which translator to invoke.
    /// </summary>
    public class TranslationModeProvider
    {
        /// <summary>
        /// List of all supported translation modes.
        /// Each mode includes:
        /// - Id: unique identifier used to select the translator
        /// - Label: text shown in the dropdown
        /// - From: source language
        /// - To: target language
        /// </summary>
        public List<TranslationMode> Modes { get; } = new()
        {
            // Braille
            new TranslationMode { Id="english_to_braille", Label="English <-> Braille", From="english", To="braille" },
            new TranslationMode { Id="braille_to_english", Label="English <-> Braille", From="braille", To="english" },

            // Minion
            new TranslationMode { Id="english_to_minion", Label="English <-> Minion", From="english", To="minion" },
            new TranslationMode { Id="minion_to_english", Label="English <-> Minion", From="minion", To="english" },

            // Morse
            new TranslationMode { Id="english_to_morse", Label="English <-> Morse", From="english", To="morse" },
            new TranslationMode { Id="morse_to_english", Label="English <-> Morse", From="morse", To="english" },

            // Orc
            new TranslationMode { Id="english_to_orc", Label="English <-> Orc", From="english", To="orc" },
            new TranslationMode { Id="orc_to_english", Label="English <-> Orc", From="orc", To="english" },
        };

        /// <summary>
        /// The default translation mode used when the application loads.
        /// </summary>
        public TranslationMode Default => Modes.First(m => m.Id == "english_to_morse");
    }
}
