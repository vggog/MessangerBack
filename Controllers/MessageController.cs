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

[Route("api/Messages")]
[ApiController]
[Authorize]
public class MessageController : ControllerBase
{
    IMessageService _service;

    public MessageController(IMessageService service) 
    { 
        _service = service;
    }

    [HttpGet]
    async public Task<IActionResult> GetChatMessages(Guid chatId)
    {
        var rawUserId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = Guid.Parse(rawUserId);

        List<MessageResponce> responce;
        try
        {
            responce = await _service.GetChatMessages(chatId, userId);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }

        return Ok(responce);
    }
}
