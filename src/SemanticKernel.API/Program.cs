using SemanticKernel.API.Options;

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

        builder.Services.AddControllers();
            
        var app = builder.Build();

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}