using SemanticKernel.Domain.Models;

namespace SemanticKernel.Domain.Interfaces;

public interface IChatHistoryService
{
    Task SaveChatHistoryAsync(ChatHistory chatHistory);
    Task<ChatHistory?> LoadChatHistoryAsync(string fileName);
    Task<List<string>> GetAvailableChatHistoriesAsync();
} 