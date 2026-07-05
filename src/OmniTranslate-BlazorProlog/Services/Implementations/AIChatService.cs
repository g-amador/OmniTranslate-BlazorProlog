using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using OmniTranslate_BlazorProlog.Services.Interfaces;

namespace OmniTranslate_BlazorProlog.Services.Implementations
{
    /// <summary>
    /// Azure OpenAI chat service implementation.
    /// </summary>
    public class AiChatService : IAIChatService
    {
        private IChatClient? _client;
        private readonly string _endpoint;
        private readonly string _apiKey;
        private readonly string _deployment;

        // Allow tests to inject a mock client
        public IChatClient? TestClient
        {
            set => _client = value;
        }

        /// <summary>
        /// Initializes the Azure OpenAI chat service using configuration values.
        /// </summary>
        /// <param name="config">Application configuration.</param>
        public AiChatService(IConfiguration config)
        {
            var settings = config.GetSection("AzureOpenAI");

            _endpoint = settings["Endpoint"] ?? "";
            _apiKey = settings["ApiKey"] ?? "";
            _deployment = settings["Deployment"] ?? "";

            // Missing config
            if (string.IsNullOrWhiteSpace(_endpoint) ||
                string.IsNullOrWhiteSpace(_apiKey) ||
                string.IsNullOrWhiteSpace(_deployment))
            {
                return;
            }

            // Placeholder detection
            if (_endpoint.Contains("YOUR-RESOURCE-NAME") ||
                _apiKey.Contains("YOUR-AZURE-OPENAI-KEY") ||
                _deployment.Contains("YOUR-MODEL-DEPLOYMENT-NAME"))
            {
                return;
            }

            // Authentication for Azure OpenAI with API key
            var client = new AzureOpenAIClient(
                new Uri(_endpoint),
                new AzureKeyCredential(_apiKey)
            );

            _client = client.GetChatClient(_deployment).AsIChatClient();
        }

        /// <inheritdoc/>
        public async Task<string> AskAsync(string prompt)
        {
            // Missing config
            if (_client == null)
            {
                return "⚠️ Azure OpenAI is not configured correctly. Please check your settings.";
            }

            try
            {
                var response = await _client.GetResponseAsync(
                    new[] { new ChatMessage(ChatRole.User, prompt) }
                );

                return response.Text ?? "⚠️ No response returned from Azure OpenAI.";
            }
            catch (Exception ex)
            {
                // Friendly error message returned to UI
                return $"⚠️ Azure OpenAI request failed: {ex.Message}";
            }
        }
    }
}
