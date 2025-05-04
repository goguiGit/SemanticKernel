namespace SemanticKernel.Domain.Interfaces;

public interface ISemanticKernelApp
{
    ISemanticKernelSession CreateSession(Guid sessionId);
    Task<ISemanticKernelSession> GetSession(Guid sessionId);
}