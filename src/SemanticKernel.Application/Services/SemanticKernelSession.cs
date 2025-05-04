using System.Text;
using Microsoft.SemanticKernel;
using SemanticKernel.Domain;
using SemanticKernel.Domain.Interfaces;

namespace SemanticKernel.Application.Services;

internal class SemanticKernelSession : ISemanticKernelSession
{
    private readonly Kernel _kernel;
    private readonly IStateStore<string> _stateStore;

    public Guid Id { get; }

    internal SemanticKernelSession(Kernel kernel, IStateStore<string> stateStore, Guid sessionId)
    {
        _kernel = kernel;
        _stateStore = stateStore;
        Id = sessionId;
    }

    private const string Prompt = @"
        ChatBot can have a conversation with you about any topic.
        It can give explicit instructions or say 'I don't know' if it does not know the answer.

        {{$history}}
        User: {{$userInput}}
        ChatBot:";

    public async Task<ChatCompletion> ProcessRequest(ChatRequest message)
    {
        var chatFunction = _kernel.CreateFunctionFromPrompt(Prompt);
        var userInput = message.Messages.Last();
        var history = await _stateStore.GetStateAsync(Id) ?? "";
        var arguments = new KernelArguments
        {
            ["history"] = history,
            ["userInput"] = userInput.Content,
        };
        var botResponse = await chatFunction.InvokeAsync(_kernel, arguments);
        var updatedHistory = $"{history}\nUser: {userInput.Content}\nChatBot: {botResponse}";
        await _stateStore.SetStateAsync(Id, updatedHistory);
        return new ChatCompletion(Message: new ChatMessage
        {
            Role = ChatRole.Assistant,
            Content = $"{botResponse}",
        })
        {
            SessionState = Id,
        };
    }

    public async IAsyncEnumerable<ChatCompletionDelta> ProcessStreamingRequest(ChatRequest message)
    {
        var chatFunction = _kernel.CreateFunctionFromPrompt(Prompt);
        var userInput = message.Messages.Last();
        var history = await _stateStore.GetStateAsync(Id) ?? "";
        var arguments = new KernelArguments
        {
            ["history"] = history,
            ["userInput"] = userInput.Content,
        };
        var streamedBotResponse = chatFunction.InvokeStreamingAsync(_kernel, arguments);
        StringBuilder response = new();
        await foreach (var botResponse in streamedBotResponse)
        {
            response.Append(botResponse);
            yield return new ChatCompletionDelta(Delta: new ChatMessageDelta
            {
                Role = ChatRole.Assistant,
                Content = $"{botResponse}",
            })
            {
                SessionState = Id,
            };
        }
        var updatedHistory = $"{history}\nUser: {userInput.Content}\nChatBot: {response}";
        await _stateStore.SetStateAsync(Id, updatedHistory);
    }

}