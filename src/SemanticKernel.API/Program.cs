using SemanticKernel.API.Interfaces;
using SemanticKernel.API.Options;
using SemanticKernel.API.Services;
using System.Text.Json.Serialization;
using System.Text.Json;
using SemanticKernel.API.Models;

namespace SemanticKernel.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        // Add services to the container.
        builder.Services.AddOptions<OpenIdSettings>()
            .Configure(settings =>
            {
                settings.DeploymentName = configuration.GetValue<string>("DeploymentName")!;
                settings.Model = configuration.GetValue<string>("Model")!;
                settings.EndPointUrl = configuration.GetValue<string>("EndPointUrl")!;
                settings.OpenIdKey = configuration.GetValue<string>("OpenIdKey")!;
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddSingleton<IStateStore<string>>(new InMemoryStore<string>());
        builder.Services.AddSingleton<ISemanticKernelApp, SemanticKernelApp>();

        // Add CORS services
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        builder.Services.AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions
                .Converters
                .Add(new JsonStringEnumConverter<ChatRole>(JsonNamingPolicy.CamelCase)));
            
        var app = builder.Build();

        app.UseHttpsRedirection();
        
        // Enable CORS
        app.UseCors();
        
        app.MapControllers();

        app.Run();
    }
}