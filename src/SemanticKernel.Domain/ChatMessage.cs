﻿using System.Text.Json.Serialization;

namespace SemanticKernel.Domain;

public struct ChatMessage
{
    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("role")]
    public ChatRole Role { get; set; }

    [JsonPropertyName("context")]
    public BinaryData? Context { get; set; }
}