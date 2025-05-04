using System.Text.Json.Serialization;

namespace SemanticKernel.Domain.Models;

public class ChatHistory
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("messages")]
    public List<ChatMessage> Messages { get; set; } = new();

    [JsonPropertyName("sessionState")]
    public string? SessionState { get; set; }
} 