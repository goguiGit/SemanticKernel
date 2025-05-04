using System.Text.Json.Serialization;
using SemanticKernel.Domain.Converters;

namespace SemanticKernel.Domain;

[JsonConverter(typeof(JsonCamelCaseEnumConverter<ChatRole>))]
public enum ChatRole
{
    System,
    Assistant,
    User
}