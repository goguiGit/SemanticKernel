namespace SemanticKernel.API.Interfaces;

public interface ISemanticKernelApp
{
    ISemanticKernelSession CreateSession(Guid sessionId);
    Task<ISemanticKernelSession> GetSession(Guid sessionId);
}