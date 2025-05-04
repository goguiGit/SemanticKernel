using SemanticKernel.API.Models;

namespace SemanticKernel.API.Interfaces;

public interface ISemanticKernelSession
{
    Guid Id { get; }
    Task<ChatCompletion> ProcessRequest(ChatRequest request);
    IAsyncEnumerable<ChatCompletionDelta> ProcessStreamingRequest(ChatRequest request);
}