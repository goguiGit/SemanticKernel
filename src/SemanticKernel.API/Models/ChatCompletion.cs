using System.Text.Json.Serialization;

namespace SemanticKernel.API.Models;

public record ChatCompletion([property: JsonPropertyName("message")] ChatMessage Message)
{
    [JsonInclude, JsonPropertyName("sessionState"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? SessionState;

    [JsonInclude, JsonPropertyName("context"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BinaryData? Context;
}