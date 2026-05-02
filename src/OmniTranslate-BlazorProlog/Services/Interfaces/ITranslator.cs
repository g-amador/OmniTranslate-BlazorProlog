namespace OmniTranslate_BlazorProlog.Services.Interfaces
{
    /// <summary>
    /// Defines a generic translator capable of converting text
    /// from one language or encoding to another.
    /// Implementations provide the translation logic and metadata.
    /// </summary>
    public interface ITranslator
    {
        /// <summary>
        /// Unique identifier for the translator.
        /// Typically formatted as "english2morse" or similar.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The source language or encoding name.
        /// Example: "english".
        /// </summary>
        string From { get; }

        /// <summary>
        /// The target language or encoding name.
        /// Example: "morse".
        /// </summary>
        string To { get; }

        /// <summary>
        /// Translates the specified input text from the <see cref="From"/>
        /// language into the <see cref="To"/> language.
        /// </summary>
        /// <param name="input">The text to translate.</param>
        /// <returns>The translated output text.</returns>
        string Translate(string input);
    }
}
