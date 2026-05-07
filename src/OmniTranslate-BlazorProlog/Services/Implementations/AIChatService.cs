using OmniTranslate_BlazorProlog.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OmniTranslate_BlazorProlog.Services.Implementations
{
    /// <summary>
    /// Azure OpenAI chat service implementation.
    /// </summary>
    public class AiChatService : IAIChatService
    {
        private readonly HttpClient _http;
        private readonly string _endpoint;
        private readonly string _apiKey;
        private readonly string _deployment;
        private readonly string _apiVersion;

        /// <summary>
        /// Initializes the Azure OpenAI chat service using configuration values.
        /// </summary>
        /// <param name="config">Application configuration.</param>
        /// <param name="http">Injected HttpClient instance.</param>
        public AiChatService(IConfiguration config, HttpClient http)
        {
            _http = http;

            var settings = config.GetSection("AzureOpenAI");

            _endpoint = settings["Endpoint"] ?? throw new Exception("Missing AzureOpenAI:Endpoint");
            _apiKey = settings["ApiKey"] ?? throw new Exception("Missing AzureOpenAI:ApiKey");
            _deployment = settings["Deployment"] ?? throw new Exception("Missing AzureOpenAI:Deployment");
            _apiVersion = settings["ApiVersion"] ?? "2025-01-01-preview";
        }

        /// <inheritdoc/>
        /// <inheritdoc/>
        public async Task<string> AskAsync(string prompt)
        {
            // Validate configuration before making the request
            if (string.IsNullOrWhiteSpace(_endpoint) ||
                string.IsNullOrWhiteSpace(_apiKey) ||
                string.IsNullOrWhiteSpace(_deployment))
            {
                return "⚠️ Azure OpenAI is not configured correctly. Please check your settings in appsettings.Development.json.";
            }

            // Detect placeholder values
            if (_endpoint.Contains("YOUR-RESOURCE-NAME") ||
                _apiKey.Contains("YOUR-AZURE-OPENAI-KEY") ||
                _deployment.Contains("YOUR-MODEL-DEPLOYMENT-NAME"))
            {
                return "⚠️ Azure OpenAI settings still contain placeholder values. Please update Endpoint, ApiKey, and Deployment.";
            }

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            var body = new
            {
                messages = new[]
                {
            new { role = "user", content = prompt }
        }
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{_endpoint}openai/deployments/{_deployment}/chat/completions?api-version={_apiVersion}";

            try
            {
                var response = await _http.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseJson);

                return doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();
            }
            catch (Exception ex)
            {
                // Friendly error message returned to UI
                return $"⚠️ Azure OpenAI request failed: {ex.Message}";
            }
        }
    }
}
