using System.Text.Json.Serialization;

namespace SemanticKernel.API.Models;

public record ChatRequest([property: JsonPropertyName("messages")] IList<ChatMessage> Messages)
{
    [JsonInclude, JsonPropertyName("sessionState")]
    public Guid? SessionState;

    [JsonInclude, JsonPropertyName("context")]
    public BinaryData? Context;
}