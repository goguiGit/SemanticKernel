using System.Text.Json.Serialization;

namespace SemanticKernel.Domain;

public record ChatRequest([property: JsonPropertyName("messages")] IList<ChatMessage> Messages)
{
    [JsonInclude, JsonPropertyName("sessionState")]
    public Guid? SessionState;

    [JsonInclude, JsonPropertyName("context")]
    public BinaryData? Context;
}