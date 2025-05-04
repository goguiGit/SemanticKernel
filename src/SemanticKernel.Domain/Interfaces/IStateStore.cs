namespace SemanticKernel.Domain.Interfaces;

public interface IStateStore<T>
{
    Task<T?> GetStateAsync(Guid sessionId);
    Task SetStateAsync(Guid sessionId, T state);
    Task RemoveStateAsync(Guid sessionId);
}