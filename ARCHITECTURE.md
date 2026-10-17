# 📁 Project Architecture

```text
Omnitranslate-blazorprolog/
├── AI.md                                                 # Provides a ready‑to‑use prompt for generating Prolog dictionaries and translator classes using external AI tools
├── ARCHITECTURE.md                                       # Detailed project architecture & directory structure
├── CONTRIBUTING.md                                       # Explains how to add new languages, translators, and API endpoints
├── LICENSE                                               # MIT license
├── README.md                                             # Project documentation
└── src/
    ├── Directory.Packages.props                          # Centralized NuGet package version management
    ├── nuget.config                                      # NuGet feed configuration
    ├── OmniTranslate-BlazorProlog.slnx                   # Solution file
    ├── OmniTranslate-BlazorProlog/
    │   ├── appsettings.Development.json                  # Development environment config
    │   ├── appsettings.json                              # Application configuration
    │   ├── OmniTranslate-BlazorProlog.csproj             # Project file
    │   ├── Program.cs                                    # Application entry point
    │   ├── Components/                                   # Blazor UI components
    │   │   ├── _Imports.razor                            # Shared Razor imports
    │   │   ├── App.razor                                 # Blazor application root
    │   │   ├── Routes.razor                              # Application routing
    │   │   ├── Layout/                                   # Application layout components
    │   │   │   ├── MainLayout.razor                      # Main layout wrapper
    │   │   │   ├── MainLayout.razor.css                  # Layout styling
    │   │   │   ├── ReconnectModal.razor                  # Connection recovery UI
    │   │   │   ├── ReconnectModal.razor.css              # Modal styling
    │   │   │   └── ReconnectModal.razor.js               # Modal behavior script
    │   │   ├── Pages/                                    # Route-level pages
    │   │   │   ├── Error.razor                           # Error page
    │   │   │   ├── NotFound.razor                        # 404 page
    │   │   │   └── TranslatorPage.razor                  # Main translation interface
    │   │   ├── Shared/                                   # Shared components
    │   │   │   └── ChatPanel.razor                       # AI chat assistant panel
    │   │   └── Translator/                               # Components used by TranslatorPage
    │   │       ├── CopyButton.razor                      # Copy-to-clipboard button
    │   │       ├── LanguageSelector.razor                # Dropdown for selecting translation mode
    │   │       └── TranslationPanel.razor                # Input/output text areas
    │   ├── Models/
    │   │   └── TranslationMode.cs                        # Enum/model defining translation modes
    │   ├── Prolog/                                       # Prolog dictionaries
    │   │   ├── braille.pl                                # Braille dictionary
    │   │   ├── minion.pl                                 # Minion dictionary
    │   │   ├── morse.pl                                  # Morse dictionary
    │   │   └── orc.pl                                    # Orc dictionary
    │   ├── Properties/
    │   │   └── launchSettings.json                       # Debug launch configuration for local development
    │   ├── Services/                                     # Application services
    │   │   ├── TranslationModeProvider.cs                # Provides active translation mode
    │   │   ├── TranslationRegistry.cs                    # Registers available translators
    │   │   ├── Implementations/                          # Concrete service implementations
    │   │   │   ├── AIChatService.cs                      # AI chat service
    │   │   │   ├── PrologTranslationService.cs           # Executes Prolog queries
    │   │   │   └── Translators/                          # C# translators (English <-> X)
    │   │   │       ├── BrailleToEnglishTranslator.cs     # Braille → English
    │   │   │       ├── EnglishToBrailleTranslator.cs     # English → Braille
    │   │   │       ├── EnglishToMinionTranslator.cs      # English → Minion
    │   │   │       ├── EnglishToMorseTranslator.cs       # English → Morse
    │   │   │       ├── EnglishToOrcTranslator.cs         # English → Orc
    │   │   │       ├── MinionToEnglishTranslator.cs      # Minion → English
    │   │   │       ├── MorseToEnglishTranslator.cs       # Morse → English
    │   │   │       └── OrcToEnglishTranslator.cs         # Orc → English
    │   │   └── Interfaces/                               # Interfaces and abstractions
    │   │       ├── IAIChatService.cs                     # AI chat interface
    │   │       ├── IPrologTranslationService.cs          # Interface for Prolog execution service
    │   │       └── ITranslator.cs                        # Base interface for all translators
    │   └── wwwroot/                                      # Static web assets
    │       ├── app.css                                   # Global stylesheet
    │       └── js/                                       # JavaScript utilities
    │           ├── clipboard.js                          # Clipboard helper for copy buttons
    │           └── screen.js                             # Auto-resize and UI utilities
    └── OmniTranslate_BlazorProlog.Tests/
        ├── AiChatServiceTests.cs                         # Tests for AI chat service
        ├── OmniTranslate_BlazorProlog.Tests.csproj       # Test project file
        └── PrologTranslationServiceTests.cs              # Tests for Prolog translation service
```
