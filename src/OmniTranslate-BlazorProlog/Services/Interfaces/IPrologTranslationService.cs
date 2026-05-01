namespace OmniTranslate_BlazorProlog.Services.Interfaces
{
    /// <summary>
    /// Defines a service capable of performing text translation
    /// using Prolog-based translation rules.
    /// </summary>
    public interface IPrologTranslationService
    {
        /// <summary>
        /// Translates the specified input text using the translation mode
        /// identified by <paramref name="mode"/>.
        /// </summary>
        /// <param name="mode">
        /// The unique identifier of the translation mode to use.
        /// Example: "english2morse".
        /// </param>
        /// <param name="input">
        /// The text to translate.
        /// </param>
        /// <returns>
        /// A task that resolves to the translated text.
        /// </returns>
        Task<string> Translate(string mode, string input);
    }
}
