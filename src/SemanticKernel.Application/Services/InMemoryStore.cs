using SemanticKernel.Domain.Interfaces;

namespace SemanticKernel.Application.Services;

public class InMemoryStore<T> : IStateStore<T>
{
    private readonly Dictionary<Guid, T> _store = new();

    public Task<T?> GetStateAsync(Guid sessionId)
    {
        _store.TryGetValue(sessionId, out var state);
        return Task.FromResult(state);
    }

    public Task SetStateAsync(Guid sessionId, T state)
    {
        _store[sessionId] = state;
        return Task.CompletedTask;
    }

    public Task RemoveStateAsync(Guid sessionId)
    {
        _store.Remove(sessionId);
        return Task.CompletedTask;
    }
}