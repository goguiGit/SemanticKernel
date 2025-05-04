using System.Text.Json;
using System.Text.Json.Serialization;

namespace SemanticKernel.Domain.Converters;

public class JsonCamelCaseEnumConverter<T> : JsonStringEnumConverter<T> where T : struct, Enum
{
    public JsonCamelCaseEnumConverter() : base(JsonNamingPolicy.CamelCase)
    {
    }
}