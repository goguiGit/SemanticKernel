namespace SemanticKernel.Domain.Interfaces;

public interface ISemanticKernelSession
{
    Guid Id { get; }
    Task<ChatCompletion> ProcessRequest(ChatRequest request);
    IAsyncEnumerable<ChatCompletionDelta> ProcessStreamingRequest(ChatRequest request);
}