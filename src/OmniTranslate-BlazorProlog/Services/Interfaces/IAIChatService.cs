namespace OmniTranslate_BlazorProlog.Services.Interfaces
{
    /// <summary>
    /// Provides an abstraction for sending chat prompts to Azure OpenAI.
    /// </summary>
    public interface IAIChatService
    {
        /// <summary>
        /// Sends a prompt to Azure OpenAI and returns the assistant's reply.
        /// </summary>
        /// <param name="prompt">The user prompt to send.</param>
        /// <returns>The assistant's response as a string.</returns>
        Task<string> AskAsync(string prompt);
    }
}