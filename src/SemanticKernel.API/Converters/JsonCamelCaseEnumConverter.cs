using System.Text.Json.Serialization;
using System.Text.Json;

namespace SemanticKernel.API.Converters;

public class JsonCamelCaseEnumConverter<T> : JsonStringEnumConverter<T> where T : struct, Enum
{
    public JsonCamelCaseEnumConverter() : base(JsonNamingPolicy.CamelCase)
    {
    }
}