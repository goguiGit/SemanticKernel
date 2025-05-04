namespace SemanticKernel.API.Interfaces;

public interface ISemanticKernelApp
{
    Task<ISemanticKernelSession> CreateSession(Guid sessionId);
    Task<ISemanticKernelSession> GetSession(Guid sessionId);
}