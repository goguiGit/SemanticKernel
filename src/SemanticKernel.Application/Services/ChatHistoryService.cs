using System.Text.Json;
using SemanticKernel.Domain.Interfaces;
using SemanticKernel.Domain.Models;

namespace SemanticKernel.Application.Services;

public class ChatHistoryService : IChatHistoryService
{
    private readonly string _basePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public ChatHistoryService()
    {
        _basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ChatHistories");
        Directory.CreateDirectory(_basePath);
        
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task SaveChatHistoryAsync(ChatHistory chatHistory)
    {
        var fileName = $"{chatHistory.Id}.json";
        var filePath = Path.Combine(_basePath, fileName);
        
        var json = JsonSerializer.Serialize(chatHistory, _jsonOptions);
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<ChatHistory?> LoadChatHistoryAsync(string fileName)
    {
        var filePath = Path.Combine(_basePath, fileName);
        if (!File.Exists(filePath))
            return null;

        var json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<ChatHistory>(json, _jsonOptions);
    }

    public Task<List<string>> GetAvailableChatHistoriesAsync()
    {
        var files = Directory.GetFiles(_basePath, "*.json")
            .Select(Path.GetFileName)
            .Where(f => f != null)
            .ToList();

        return Task.FromResult(files!);
    }
} 