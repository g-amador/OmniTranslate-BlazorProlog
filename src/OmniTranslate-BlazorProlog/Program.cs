using MudBlazor.Services;
using OmniTranslate_BlazorProlog.Components;
using OmniTranslate_BlazorProlog.Services;
using OmniTranslate_BlazorProlog.Services.Implementations;
using OmniTranslate_BlazorProlog.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// In development, enable static web assets so Razor components
/// and referenced resources are served correctly.
/// </summary>
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.UseStaticWebAssets();
}

/// <summary>
/// Registers Razor Components and enables interactive server rendering.
/// </summary>
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

/// <summary>
/// Registers MudBlazor services (dialogs, snackbars, theming, etc.).
/// </summary>
builder.Services.AddMudServices();

// Registers the central registry that discovers and stores all translators.
// This must be registered before any service that depends on it.
builder.Services.AddSingleton<TranslationRegistry>();

// Registers the Prolog-backed translation service that performs the actual translations.
// This service uses TranslationRegistry internally to look up translators by mode ID.
builder.Services.AddSingleton<IPrologTranslationService, PrologTranslationService>();

// Registers the provider responsible for exposing available translation modes to the UI.
// Depends on TranslationRegistry, so it must be registered after it.
builder.Services.AddSingleton<TranslationModeProvider>();

/// <summary>
/// Registers the Azure OpenAI chat service and injects HttpClient for API calls.
/// </summary>
builder.Services.AddScoped<IAIChatService, AiChatService>();

var app = builder.Build();

/// <summary>
/// Configures the HTTP request pipeline.
/// Production uses a custom error handler and HSTS.
/// </summary>
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

/// <summary>
/// Redirects unknown status codes (404, etc.) to the Not Found page.
/// </summary>
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

/// <summary>
/// Serves static files such as CSS, JS, images, and Prolog files.
/// </summary>
app.UseStaticFiles();

/// <summary>
/// Enables antiforgery protection for interactive components.
/// </summary>
app.UseAntiforgery();

/// <summary>
/// Maps static assets for Razor Components.
/// </summary>
app.MapStaticAssets();

/// <summary>
/// Maps the root Razor Component (App.razor) and enables
/// interactive server rendering mode.
/// </summary>
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
