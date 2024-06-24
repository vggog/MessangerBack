using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using System.Security.Claims;
using System.Net;

using MessangerBack.Models;
using MessangerBack.Repositories;
using MessangerBack.Schemas;
using MessangerBack.Services;
using MessangerBack.Exceptions;
using MessangerBack.Responces;


namespace MessangerBack.Controllers;

[Route("api/Chats")]
[ApiController]
[Authorize]
public class ChatsController : ControllerBase
{
    IChatService _service;

    public ChatsController(IChatService service) 
    { 
        _service = service;
    }

    [HttpPost]
    async public Task<IActionResult> CreateChat([FromBody] CreateChatRequestSchema createChatData)
    {
        var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

        var token = Guid.Parse(userId);
        await _service.CreateChat(token, createChatData.ChatName);

        return Created();
    }
}
