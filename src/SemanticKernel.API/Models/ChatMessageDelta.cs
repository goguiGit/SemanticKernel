using System.Text.Json.Serialization;

namespace SemanticKernel.API.Models;

public struct ChatMessageDelta
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("role")]
    public ChatRole? Role { get; set; }

    [JsonPropertyName("context"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BinaryData? Context { get; set; }
}