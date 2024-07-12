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
        var chat = await _service.CreateChat(token, createChatData.ChatName);

        return Ok(chat);
    }

    [HttpGet]
    [Route("All")]
    async public Task<IActionResult> AllChats(string chatName = "")
    {
        var chats = await _service.AllChatsByChatName(chatName);

        return Ok(chats);
    }

    [HttpGet]
    [Route("Add")]
    async public Task<IActionResult> AddToChat(Guid chatId)
    {
        var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var token = Guid.Parse(userId);

        try
        {
            await _service.AddToChat(token, chatId);
        } 
        catch(WrongUserInputException ex)
        {
            return BadRequest(ex.Message);
        }
        catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }

        return Ok();
    }

    [HttpGet]
    async public Task<IActionResult> GetAllChats()
    {
        var rawUserId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = Guid.Parse(rawUserId);

        var chats = await _service.GetAllUserChats(userId);

        return Ok(chats);
    }

    [HttpGet]
    [Route("Info")]
    async public Task<IActionResult> GetChatInfo(Guid chatId)
    {
        var rawUserId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = Guid.Parse(rawUserId);

        var chat = await _service.GetChatInfo(chatId);

        return Ok(chat);
    }
}
