namespace OmniTranslate_BlazorProlog.Models
{
    /// <summary>
    /// Represents a translation direction supported by the application.
    /// Each mode defines a unique identifier, a display label,
    /// and the source/target language codes.
    /// </summary>
    public class TranslationMode
    {
        /// <summary>
        /// Unique identifier for the translation mode.
        /// Typically formatted as "english_to_morse" or "morse_to_english".
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Human‑readable label shown in the UI.
        /// Example: "English <-> Morse".
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The source language or encoding name.
        /// Example: "english".
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The target language or encoding name.
        /// Example: "morse".
        /// </summary>
        public string To { get; set; }
    }
}
