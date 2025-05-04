using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using SemanticKernel.Application.Options;
using SemanticKernel.Domain.Interfaces;

namespace SemanticKernel.Application.Services;

public class SemanticKernelApp : ISemanticKernelApp
{
    private readonly IStateStore<string> _stateStore;
    private readonly Lazy<Kernel> _kernel;
    private readonly IOptions<AzureOpenAiSettings> _options;


    public SemanticKernelApp(IStateStore<string> stateStore, 
        IOptions<AzureOpenAiSettings> options)
    {
        _stateStore = stateStore;
        _options = options;
        _kernel = new Lazy<Kernel>(Init());
    }

    public ISemanticKernelSession CreateSession(Guid sessionId)
    {
        var kernel = _kernel.Value;
        return new SemanticKernelSession(kernel, _stateStore, sessionId);
    }

    public async Task<ISemanticKernelSession> GetSession(Guid sessionId)
    {
        var kernel = _kernel.Value;
        var state = await _stateStore.GetStateAsync(sessionId);
        if (state is null)
        {
            throw new KeyNotFoundException($"Session {sessionId} not found.");
        }
        return new SemanticKernelSession(kernel, _stateStore, sessionId);
    }

    private  Kernel Init()
    {
        return Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(_options.Value.DeploymentName, 
                new AzureOpenAIClient(new Uri(_options.Value.EndPointUrl), 
                    new ApiKeyCredential(_options.Value.OpenIdKey)), 
                modelId: _options.Value.Model)
            .Build();
    }
}