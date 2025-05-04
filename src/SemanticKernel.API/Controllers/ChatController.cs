using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using SemanticKernel.Domain;
using SemanticKernel.Domain.Interfaces;

[ApiController, Route("api/[controller]")]
public class ChatController(ISemanticKernelApp semanticKernelApp) : ControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> ProcessMessage(ChatRequest request)
    {
        var session = request.SessionState switch
        {
            { } sessionId => await semanticKernelApp.GetSession(sessionId),
            _ => semanticKernelApp.CreateSession(Guid.NewGuid())
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
            { } sessionId => await semanticKernelApp.GetSession(sessionId),
            _ => semanticKernelApp.CreateSession(Guid.NewGuid())
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
