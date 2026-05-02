using OmniTranslate_BlazorProlog.Services.Implementations.Translators;
using OmniTranslate_BlazorProlog.Services.Interfaces;

namespace OmniTranslate_BlazorProlog.Services
{
    /// <summary>
    /// Central registry that stores all available translators.
    /// Each translator is registered by its unique ID and can be retrieved at runtime.
    /// </summary>
    public class TranslationRegistry
    {
        /// <summary>
        /// Dictionary of translators indexed by their unique ID.
        /// Example: "english_to_braille" → EnglishToBrailleTranslator instance.
        /// </summary>
        public Dictionary<string, ITranslator> Translators { get; } = new();

        /// <summary>
        /// Registers a translator instance using its <see cref="ITranslator.Id"/> as the key.
        /// If a translator with the same ID already exists, it will be replaced.
        /// </summary>
        /// <param name="translator">The translator instance to register.</param>
        public void Register(ITranslator translator)
        {
            Translators[translator.Id] = translator;
        }

        /// <summary>
        /// Initializes the registry and registers all built‑in translators.
        /// Add new translators here to make them available to the application.
        /// </summary>
        public TranslationRegistry()
        {
            // Braille translators
            Register(new EnglishToBrailleTranslator());
            Register(new BrailleToEnglishTranslator());

            // Minion translators
            Register(new EnglishToMinionTranslator());
            Register(new MinionToEnglishTranslator());

            // Morse translators
            Register(new EnglishToMorseTranslator());
            Register(new MorseToEnglishTranslator());

            // Orc translators
            Register(new EnglishToOrcTranslator());
            Register(new OrcToEnglishTranslator());
        }
    }
}
