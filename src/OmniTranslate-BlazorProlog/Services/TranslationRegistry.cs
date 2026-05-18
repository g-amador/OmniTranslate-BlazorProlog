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
        /// Scans the assembly for all classes implementing <see cref="ITranslator"/>,
        /// creates an instance of each one, and registers them by their unique ID.
        // Set loadDefaults to false during unit tests to avoid loading real Prolog translators.
        /// </summary>
        public TranslationRegistry(bool loadDefaults = true)
        {
            if (!loadDefaults)
            {
                return; // Skip loading real translators in tests
            }
            return; // Skip loading real translators in tests
            // Find all types in the assembly that implement ITranslator.
            // We exclude interfaces and abstract classes because they cannot be instantiated.
            var translators = typeof(ITranslator).Assembly
                .GetTypes()
                .Where(t => typeof(ITranslator).IsAssignableFrom(t) &&
                            !t.IsInterface &&
                            !t.IsAbstract);

            // Create an instance of each translator and register it by its ID.
            foreach (var type in translators)
            {
                // Activator.CreateInstance creates the translator using its parameterless constructor.
                var instance = (ITranslator)Activator.CreateInstance(type)!;

                // Store the translator in the dictionary using its unique ID.
                Translators[instance.Id] = instance;
            }
        }
    }
}
