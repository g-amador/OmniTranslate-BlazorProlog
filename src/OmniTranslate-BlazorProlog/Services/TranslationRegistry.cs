using OmniTranslate_BlazorProlog.Services.Implementations;
using OmniTranslate_BlazorProlog.Services.Interfaces;

namespace OmniTranslate_BlazorProlog.Services
{
    public class TranslationRegistry
    {
        public Dictionary<string, ITranslator> Translators { get; } = new();

        public TranslationRegistry()
        {
            Register(new MorseToEnglishTranslator());
            Register(new EnglishToMorseTranslator());
        }

        private void Register(ITranslator t)
        {
            Translators[t.Id] = t;
        }
    }
}
