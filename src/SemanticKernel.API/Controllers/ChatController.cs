using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly Kernel _kernel;

    public ChatController(Kernel kernel)
    {
        _kernel = kernel;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ChatRequest request)
    {
        var chat = _kernel.GetRequiredService<IChatCompletionService>();
        var history = new ChatHistory("Eres un asistente útil.");

        foreach (var pair in request.History)
        {
            history.AddUserMessage(pair.User);
            history.AddAssistantMessage(pair.Assistant);
        }

        history.AddUserMessage(request.Message);
        var result = await chat.GetChatMessageAsync(history);

        return Ok(new { reply = result.Content });
    }
}

public class ChatRequest
{
    public string Message { get; set; } = "";
    public List<ChatMessagePair> History { get; set; } = new();
}

public class ChatMessagePair
{
    public string User { get; set; } = "";
    public string Assistant { get; set; } = "";
}