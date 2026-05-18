using OmniTranslate_BlazorProlog.Services;
using OmniTranslate_BlazorProlog.Services.Implementations;
using OmniTranslate_BlazorProlog.Services.Interfaces;

namespace OmniTranslate_BlazorProlog.Tests
{
    /// <summary>
    /// Unit tests for <see cref="PrologTranslationService"/>.
    /// Validates behavior for empty input, unknown modes,
    /// and correct translator routing using a test-friendly registry.
    /// </summary>
    public class PrologTranslationServiceTests
    {
        /// <summary>
        /// Ensures the service returns an empty string when the input text is empty.
        /// </summary>
        [Fact]
        public async Task Translate_ReturnsEmpty_WhenInputEmpty()
        {
            // Use a registry without default translators
            var registry = new TranslationRegistry(loadDefaults: false);
            var service = new PrologTranslationService(registry);

            var result = await service.Translate("english_to_morse", "");

            Assert.Equal("", result);
        }

        /// <summary>
        /// Ensures the service returns an empty string when the translation mode
        /// does not exist in the registry.
        /// </summary>
        [Fact]
        public async Task Translate_ReturnsEmpty_WhenModeUnknown()
        {
            var registry = new TranslationRegistry(loadDefaults: false);
            var service = new PrologTranslationService(registry);

            var result = await service.Translate("unknown_mode", "hello");

            Assert.Equal("", result);
        }

        /// <summary>
        /// Ensures the service correctly routes translation requests to the
        /// appropriate <see cref="ITranslator"/> implementation.
        /// </summary>
        [Fact]
        public async Task Translate_UsesCorrectTranslator()
        {
            // Create a registry without loading real Prolog translators
            var registry = new TranslationRegistry(loadDefaults: false);

            // Register a fake translator for testing
            registry.Translators["test_mode"] = new FakeTranslator();

            var service = new PrologTranslationService(registry);

            var result = await service.Translate("test_mode", "hello");

            Assert.Equal("Translated: hello", result);
        }
    }

    /// <summary>
    /// Simple fake translator used for testing translation routing.
    /// </summary>
    public class FakeTranslator : ITranslator
    {
        public string Id => "test_mode";
        public string From => "test";
        public string To => "test";
        public string Label => "Test Translator";

        public string Translate(string input)
        {
            return $"Translated: {input}";
        }
    }
}
