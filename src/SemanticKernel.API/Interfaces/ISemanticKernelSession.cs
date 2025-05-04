using SemanticKernel.API.Models;
using ChatCompletion = OpenAI.Chat.ChatCompletion;

namespace SemanticKernel.API.Interfaces;

public interface ISemanticKernelSession
{
    Guid Id { get; }
    Task<ChatCompletion> ProcessRequest(ChatRequest request);
    IAsyncEnumerable<ChatCompletionDelta> ProcessStreamingRequest(ChatRequest request);
}