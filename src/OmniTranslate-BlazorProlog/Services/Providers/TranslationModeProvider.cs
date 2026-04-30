using OmniTranslate_BlazorProlog.Models;

namespace OmniTranslate_BlazorProlog.Services.Providers
{
    public class TranslationModeProvider
    {
        public List<TranslationMode> Modes { get; } = new()
    {
        new TranslationMode { Id="english2braille", Label="English → Braille", From="english", To="braille" },
        new TranslationMode { Id="braille2english", Label="Braille → English", From="braille", To="english" },

        new TranslationMode { Id="english2minion", Label="English → Minion", From="english", To="minion" },
        new TranslationMode { Id="minion2english", Label="Minion → English", From="minion", To="english" },

        new TranslationMode { Id="english2morse", Label="English → Morse", From="english", To="morse" },
        new TranslationMode { Id="morse2english", Label="Morse → English", From="morse", To="english" },

        new TranslationMode { Id="english2orc", Label="English → Orc", From="english", To="orc" },
        new TranslationMode { Id="orc2english", Label="Orc → English", From="orc", To="english" },
    };

        public TranslationMode Default => Modes.First(m => m.Id == "english2morse");
    }

}
