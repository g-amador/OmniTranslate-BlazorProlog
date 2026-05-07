namespace OmniTranslate_BlazorProlog.Services.Interfaces
{
    /// <summary>
    /// Represents a translator capable of converting text between two
    /// languages or encodings (e.g., English ↔ Morse, English ↔ Orc).
    /// 
    /// Each implementation defines:
    /// - A unique <see cref="Id"/> used for routing translation requests.
    /// - The <see cref="From"/> and <see cref="To"/> language identifiers.
    /// - A human‑readable <see cref="Label"/> for UI display.
    /// - The translation logic via <see cref="Translate(string)"/>.
    /// </summary>
    public interface ITranslator
    {
        /// <summary>
        /// Unique identifier for this translator.
        /// 
        /// This value is used internally by:
        /// - <see cref="TranslationRegistry"/> to register the translator.
        /// - <see cref="TranslationModeProvider"/> to expose it to the UI.
        /// 
        /// Convention: lowercase, snake_case, e.g. "english_to_morse".
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The source language or encoding name.
        /// Examples: "english", "morse", "orc".
        /// 
        /// This value is used by the UI and by translation mode metadata.
        /// </summary>
        string From { get; }

        /// <summary>
        /// The target language or encoding name.
        /// Examples: "braille", "minion", "english".
        /// 
        /// This value determines the output format of the translator.
        /// </summary>
        string To { get; }

        /// <summary>
        /// Human‑readable label describing this translation direction.
        /// 
        /// Example: "English <-> Morse".
        /// 
        /// This is displayed in the UI dropdown and should be concise
        /// and user‑friendly. Implementations may generate this dynamically
        /// or return a fixed string.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Translates the specified input text from the <see cref="From"/>
        /// language into the <see cref="To"/> language.
        /// 
        /// Implementations should ensure:
        /// - Input is processed safely.
        /// - Output follows the expected encoding rules.
        /// - Errors are handled gracefully (e.g., unknown symbols).
        /// </summary>
        /// <param name="input">The text to translate.</param>
        /// <returns>The translated output text.</returns>
        string Translate(string input);
    }
}
