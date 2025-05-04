using System.Text.Json.Serialization;
using SemanticKernel.API.Converters;

namespace SemanticKernel.API.Models;

[JsonConverter(typeof(JsonCamelCaseEnumConverter<ChatRole>))]
public enum ChatRole
{
    System,
    Assistant,
    User
}