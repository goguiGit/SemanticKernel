using Microsoft.AspNetCore.Mvc;
using SemanticKernel.Domain.Interfaces;
using SemanticKernel.Domain.Models;

namespace SemanticKernel.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatHistoryController(IChatHistoryService chatHistoryService) : ControllerBase
{
    [HttpPost("save")]
    public async Task<IActionResult> SaveChatHistory([FromBody] ChatHistory chatHistory)
    {
        await chatHistoryService.SaveChatHistoryAsync(chatHistory);
        return Ok(new { message = "Chat history saved successfully" });
    }

    [HttpGet("load/{fileName}")]
    public async Task<IActionResult> LoadChatHistory(string fileName)
    {
        var chatHistory = await chatHistoryService.LoadChatHistoryAsync(fileName);
        if (chatHistory == null)
            return NotFound();

        return Ok(chatHistory);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAvailableChatHistories()
    {
        var histories = await chatHistoryService.GetAvailableChatHistoriesAsync();
        return Ok(histories);
    }
} 