using OmniTranslate_BlazorProlog.Models;

namespace OmniTranslate_BlazorProlog.Services
{
    public class TranslationModeProvider
    {
        public List<TranslationMode> Modes { get; } = new()
        {
            new TranslationMode { Id="english_to_braille", Label="English <-> Braille", From="english", To="braille" },
            new TranslationMode { Id="braille_to_english", Label="English <-> Braille", From="braille", To="english" },

            new TranslationMode { Id="english_to_minion", Label="English <-> Minion", From="english", To="minion" },
            new TranslationMode { Id="minion_to_english", Label="English <-> Minion", From="minion", To="english" },

            new TranslationMode { Id="english_to_morse", Label="English <-> Morse", From="english", To="morse" },
            new TranslationMode { Id="morse_to_english", Label="English <-> Morse", From="morse", To="english" },

            new TranslationMode { Id="english_to_orc", Label="English <-> Orc", From="english", To="orc" },
            new TranslationMode { Id="orc_to_english", Label="English <-> Orc", From="orc", To="english" },
        };

        public TranslationMode Default => Modes.First(m => m.Id == "english_to_morse");
    }
}
