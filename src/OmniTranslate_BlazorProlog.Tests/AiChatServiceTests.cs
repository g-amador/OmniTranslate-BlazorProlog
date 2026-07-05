using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Moq;
using OmniTranslate_BlazorProlog.Services.Implementations;

namespace OmniTranslate_BlazorProlog.Tests
{
    /// <summary>
    /// Unit tests for <see cref="AiChatService"/>.
    /// Ensures correct behavior for configuration validation,
    /// placeholder detection, successful responses, and error handling.
    /// </summary>
    public class AiChatServiceTests
    {
        // Builds an IConfiguration instance with the provided Azure OpenAI settings.
        private IConfiguration BuildConfig(string endpoint, string key, string deployment)
        {
            var inMemory = new Dictionary<string, string?>
            {
                ["AzureOpenAI:Endpoint"] = endpoint,
                ["AzureOpenAI:ApiKey"] = key,
                ["AzureOpenAI:Deployment"] = deployment,
                ["AzureOpenAI:ApiVersion"] = "2025-01-01-preview"
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemory)
                .Build();
        }

        /// <summary>
        /// Ensures the service returns a warning when required configuration
        /// values are missing.
        /// </summary>
        [Fact]
        public async Task AskAsync_ReturnsWarning_WhenConfigMissing()
        {
            var config = BuildConfig("", "key", "deployment");
            var service = new AiChatService(config);

            var result = await service.AskAsync("Hello");

            Assert.Contains("not configured correctly", result);
        }

        /// <summary>
        /// Ensures the service detects wrong configuration values
        /// and returns a helpful warning.
        /// </summary>
        [Fact]
        public async Task AskAsync_ReturnsWarning_WhenWrongConfigurationUsed()
        {
            var config = BuildConfig(
                "YOUR-RESOURCE-NAME",
                "YOUR-AZURE-OPENAI-KEY",
                "YOUR-MODEL-DEPLOYMENT-NAME"
            );

            var service = new AiChatService(config);

            var result = await service.AskAsync("Hello");

            Assert.Contains("not configured correctly.", result);
        }

        /// <summary>
        /// Ensures the service correctly parses a valid Azure OpenAI response
        /// and returns the assistant's message content.
        /// </summary>
        [Fact]
        public async Task AskAsync_ReturnsParsedResponse_WhenSuccess()
        {
            var mockClient = new Mock<IChatClient>();
            mockClient
                .Setup(c => c.GetResponseAsync(
                    It.IsAny<IEnumerable<ChatMessage>>(),
                    It.IsAny<ChatOptions?>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new ChatResponse(new ChatMessage(new ChatRole("user"), "Hello from Azure!")));


            var config = BuildConfig("https://example.com/", "key", "deployment");

            var service = new AiChatService(config)
            {
                TestClient = mockClient.Object
            };

            var result = await service.AskAsync("Hello");

            Assert.Equal("Hello from Azure!", result);
        }

        /// <summary>
        /// Ensures the service returns a friendly error message when an exception
        /// occurs during the HTTP request.
        /// </summary>
        [Fact]
        public async Task AskAsync_ReturnsFriendlyError_OnException()
        {
            var mockClient = new Mock<IChatClient>();
            mockClient
                .Setup(c => c.GetResponseAsync(
                    It.IsAny<IEnumerable<ChatMessage>>(),
                    null,
                    default
                ))
                .ThrowsAsync(new Exception("Network down"));

            var config = BuildConfig("https://example.com/", "key", "deployment");

            var service = new AiChatService(config)
            {
                TestClient = mockClient.Object
            };

            var result = await service.AskAsync("Hello");

            Assert.Contains("request failed", result);
        }
    }
}
