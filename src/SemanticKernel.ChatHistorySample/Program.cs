using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI.Chat;
using ChatMessageContent = Microsoft.SemanticKernel.ChatMessageContent;

namespace SemanticKernel.ChatHistorySample;

internal class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets("SemanticKernel.Secrets")
            .Build();

        var endpoint = new Uri(config["EndPointUrl"]);
        var model = config["Model"];
        var deploymentName = config["DeploymentName"];
        var openIdKey = config["OpenIdKey"];

        var kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(deploymentName, new AzureOpenAIClient(endpoint, new ApiKeyCredential(openIdKey)), modelId: model)
            .Build();

        var chatCompletionService = kernel.Services.GetRequiredService<IChatCompletionService>();

        // Create a new chat history
        var chatHistory = new ChatHistory();

        Console.ForegroundColor = ConsoleColor.Green;

        // Add user message to chat history
        var message1 = "Write a hello world program in C#";
        Console.WriteLine(message1);
        chatHistory.AddUserMessage(message1);

        var answer1 = await chatCompletionService.GetChatMessageContentAsync(chatHistory);

        Console.WriteLine("Press any key to continue");
        Console.WriteLine(answer1);
        WriteTokenUsage(answer1);
        Console.ReadKey();

        //Add the first answer to the history
        chatHistory.AddAssistantMessage(answer1.ToString());

        Console.ForegroundColor = ConsoleColor.Blue;

        // Add another user message to chat history
        var message2 = "Rewrite previous answer in VB.NET";
        Console.WriteLine(message2);
        chatHistory.AddUserMessage(message2);

        var answer2 = await chatCompletionService.GetChatMessageContentAsync(chatHistory);

        Console.WriteLine("Press any key to continue");
        Console.WriteLine(answer2);
        WriteTokenUsage(answer2);
        Console.ReadKey();


    }

    private static void WriteTokenUsage(ChatMessageContent response)
    {
        if (response?.Metadata == null) return;

        if (response.Metadata.TryGetValue("Usage", out var usage) && usage != null)
        {
            if (usage is ChatTokenUsage usageDict)
            {
                Console.WriteLine($"Input Tokens: {usageDict.InputTokenCount}");
                Console.WriteLine($"Output Tokens: {usageDict.OutputTokenCount}");
                Console.WriteLine($"Total Tokens: {usageDict.TotalTokenCount}");
            }
        }
    }
}