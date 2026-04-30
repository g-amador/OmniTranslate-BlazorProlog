using OmniTranslate_BlazorProlog.Models;

namespace OmniTranslate_BlazorProlog.Services.Providers;

public class TranslationPairProvider
{
    public List<TranslationPair> All { get; } =
    [
        new TranslationPair
        {
            DisplayName = "Text ↔ Morse",
            LeftLabel = "Text",
            RightLabel = "Morse",
            ModeLeftToRight = "text_to_morse",
            ModeRightToLeft = "morse_to_text"
        },
        new TranslationPair
        {
            DisplayName = "Text ↔ Orc",
            LeftLabel = "Text",
            RightLabel = "Orc",
            ModeLeftToRight = "text_to_orc",
            ModeRightToLeft = "orc_to_text"
        },
        new TranslationPair
        {
            DisplayName = "Text ↔ Minion",
            LeftLabel = "Text",
            RightLabel = "Minion",
            ModeLeftToRight = "text_to_minion",
            ModeRightToLeft = "minion_to_text"
        },
        new TranslationPair
        {
            DisplayName = "Text ↔ Braille",
            LeftLabel = "Text",
            RightLabel = "Braille",
            ModeLeftToRight = "text_to_braille",
            ModeRightToLeft = "braille_to_text"
        }
    ];

    public TranslationPair DefaultPair => All.First();
}
