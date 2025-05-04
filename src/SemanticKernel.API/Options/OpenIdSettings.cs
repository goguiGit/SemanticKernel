using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace SemanticKernel.API.Options;

public class OpenIdSettings
{
    [Required(ErrorMessage = "EndPointUrl is required")]
    [Url(ErrorMessage = "EndPointUrl must be a valid URL")]
    public string EndPointUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Model is required")]
    public string Model { get; set; } = string.Empty;

    [Required(ErrorMessage = "DeploymentName is required")]
    public string DeploymentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "OpenIdKey is required")]
    [MinLength(32, ErrorMessage = "OpenIdKey must be at least 32 characters long")]
    public string OpenIdKey { get; set; } = string.Empty;
} 