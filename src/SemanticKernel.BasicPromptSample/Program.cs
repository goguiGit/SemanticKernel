﻿using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace SemanticKernel.BasicPromptSample;

public class Program
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

        // Basic prompt
        var prompt1 = "Write a hello world program in C#";

        // More detailed prompt
        var prompt2 = "Write a C# console application that displays 'Hello, World!' in green text and waits for a key press before closing";

        // Very detailed prompt
        var prompt3 = "Create a C# console application that:\n" +
                     "1. Changes console text color to green\n" +
                     "2. Displays 'Hello, World!' centered on the screen\n" +
                     "3. Restores original console color\n" +
                     "4. Waits for any key press before exit\n" +
                     "Use proper exception handling";

        // Most detailed prompt
        var prompt4 = "Create a C# console application that implements these requirements:\n" +
                     "1. Use top-level statements for Program.cs\n" +
                     "2. Set console title to 'Hello World App'\n" +
                     "3. Store original console colors before changing\n" +
                     "4. Change text color to green (ConsoleColor.Green)\n" +
                     "5. Clear the console screen\n" +
                     "6. Calculate center position for text placement\n" +
                     "7. Display 'Hello, World!' centered horizontally\n" +
                     "8. Restore original console colors\n" +
                     "9. Add try-catch block for error handling\n" +
                     "10. Display 'Press any key to exit' at bottom\n" +
                     "Use .NET 9 features where applicable";

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Basic Prompt Result:");
        Console.WriteLine(await kernel.InvokePromptAsync(prompt1));
        Console.WriteLine("\nPress any key for next example...");
        Console.ReadKey();


        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Detailed Prompt Result:");
        Console.WriteLine(await kernel.InvokePromptAsync(prompt2));
        Console.WriteLine("\nPress any key for next example...");
        Console.ReadKey();


        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Very Detailed Prompt Result:");
        Console.WriteLine(await kernel.InvokePromptAsync(prompt3));
        Console.WriteLine("\nPress any key for next example...");
        Console.ReadKey();


        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Most Detailed Prompt Result:");
        Console.WriteLine(await kernel.InvokePromptAsync(prompt4));
        Console.ReadKey();
        
        Console.Clear();

        for (int i = 0; i < 7; i++)
        {
            Console.WriteLine();
        }
        Console.ForegroundColor = ConsoleColor.Magenta;


        Console.WriteLine("What's next?");
        Console.ReadKey();
        Console.WriteLine("Rewrite previous answer in VB.net:");
        Console.WriteLine(await kernel.InvokePromptAsync("Rewrite previous answer in VB.net"));
        Console.ReadKey();

    }
}