using System.Text.Json.Serialization;

namespace SemanticKernel.API.Models;

public record ChatCompletionDelta([property: JsonPropertyName("delta")] ChatMessageDelta Delta)
{
    [JsonInclude, JsonPropertyName("sessionState"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? SessionState;

    [JsonInclude, JsonPropertyName("context"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BinaryData? Context;
}