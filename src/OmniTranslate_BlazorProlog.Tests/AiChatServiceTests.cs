using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using OmniTranslate_BlazorProlog.Services.Implementations;
using System.Net;

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

        // Creates a mocked HttpClient that returns a predefined JSON response.
        private HttpClient CreateMockHttp(string responseJson, HttpStatusCode status = HttpStatusCode.OK)
        {
            var handler = new Mock<HttpMessageHandler>();

            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = status,
                    Content = new StringContent(responseJson)
                });

            return new HttpClient(handler.Object);
        }

        /// <summary>
        /// Ensures the service returns a warning when required configuration
        /// values are missing.
        /// </summary>
        [Fact]
        public async Task AskAsync_ReturnsWarning_WhenConfigMissing()
        {
            var config = BuildConfig("", "key", "deployment");
            var http = new HttpClient();

            var service = new AiChatService(config, http);

            var result = await service.AskAsync("Hello");

            Assert.Contains("not configured correctly", result);
        }

        /// <summary>
        /// Ensures the service detects placeholder configuration values
        /// and returns a helpful warning.
        /// </summary>
        [Fact]
        public async Task AskAsync_ReturnsWarning_WhenPlaceholdersUsed()
        {
            var config = BuildConfig(
                "YOUR-RESOURCE-NAME",
                "YOUR-AZURE-OPENAI-KEY",
                "YOUR-MODEL-DEPLOYMENT-NAME"
            );

            var http = new HttpClient();
            var service = new AiChatService(config, http);

            var result = await service.AskAsync("Hello");

            Assert.Contains("placeholder values", result);
        }

        /// <summary>
        /// Ensures the service correctly parses a valid Azure OpenAI response
        /// and returns the assistant's message content.
        /// </summary>
        [Fact]
        public async Task AskAsync_ReturnsParsedResponse_WhenSuccess()
        {
            var json = """
            {
                "choices": [
                    {
                        "message": { "content": "Hello from Azure!" }
                    }
                ]
            }
            """;

            var config = BuildConfig("https://example.com/", "key", "deployment");
            var http = CreateMockHttp(json);

            var service = new AiChatService(config, http);

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
            var config = BuildConfig("https://example.com/", "key", "deployment");

            // Mock handler that throws an exception
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network down"));

            var http = new HttpClient(handler.Object);
            var service = new AiChatService(config, http);

            var result = await service.AskAsync("Hello");

            Assert.Contains("request failed", result);
        }
    }
}
