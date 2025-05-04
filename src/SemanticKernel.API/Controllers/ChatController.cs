using Microsoft.AspNetCore.Mvc;
using SemanticKernel.API.Interfaces;
using SemanticKernel.API.Models;
using System.Text.Json;

[ApiController, Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ISemanticKernelApp _semanticKernelApp;

    public ChatController(ISemanticKernelApp semanticKernelApp)
    {
        _semanticKernelApp = semanticKernelApp;
    }

    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> ProcessMessage(ChatRequest request)
    {
        var session = request.SessionState switch
        {
            { } sessionId => await _semanticKernelApp.GetSession(sessionId),
            _ => await _semanticKernelApp.CreateSession(Guid.NewGuid())
        };
        var response = await session.ProcessRequest(request);
        return Ok(response);
    }

    [HttpPost("stream")]
    [Consumes("application/json")]
    public async Task ProcessStreamingMessage(ChatRequest request)
    {
        var session = request.SessionState switch
        {
            { } sessionId => await _semanticKernelApp.GetSession(sessionId),
            _ => await _semanticKernelApp.CreateSession(Guid.NewGuid())
        };
        var response = Response;
        response.Headers.Append("Content-Type", "application/x-ndjson");
        await foreach (var delta in session.ProcessStreamingRequest(request))
        {
            await response.WriteAsync($"{JsonSerializer.Serialize(delta)}\r\n");
            await response.Body.FlushAsync();
        }
    }
}
