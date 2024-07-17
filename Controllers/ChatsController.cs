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
    private readonly ILogger<ChatsController> _logger;

    public ChatsController(IChatService service, ILogger<ChatsController> logger) 
    { 
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    async public Task<IActionResult> CreateChat([FromBody] CreateChatRequestSchema createChatData)
    {
        _logger.LogInformation("Вход в метод CreateChat");

        var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

        _logger.LogInformation($"Идентификатор пользователя: {userId}");

        var token = Guid.Parse(userId);

        _logger.LogInformation($"Токен пользователя: {token}");

        var chat = await _service.CreateChat(token, createChatData.ChatName);

        _logger.LogInformation($"Создана беседа: {chat}");

        _logger.LogInformation("Выход из метода CreateChat");

        return Ok(chat);
    }

    [HttpGet]
    [Route("All")]
    async public Task<IActionResult> AllChats(string chatName = "")
    {
        _logger.LogInformation("Вход в метод AllChats");
        var chats = await _service.AllChatsByChatName(chatName);

        _logger.LogInformation("Все чаты получены");
        return Ok(chats);
    }

    [HttpGet]
    [Route("Add")]
    async public Task<IActionResult> AddToChat(Guid chatId)
    {
        _logger.LogInformation("Вход в метод AddToChat");

        var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        _logger.LogInformation($"Идентификатор пользователя: {userId}");

        var token = Guid.Parse(userId);
        _logger.LogInformation($"Токен пользователя: {token}");

        try
        {
            await _service.AddToChat(token, chatId);
            _logger.LogInformation("Пользователь успешно добавлен в беседу");
        } 
        catch(WrongUserInputException ex)
        {
            _logger.LogError(ex, "Ошибка добавления пользователя в беседу");
            return BadRequest(ex.Message);
        }
        catch(NotFoundException ex)
        {
            _logger.LogError(ex, "Ошибка добавления пользователя в беседу");
            return NotFound(ex.Message);
        }

        _logger.LogInformation("Выход из метода AddToChat");

        return Ok();
    }

    [HttpGet]
    async public Task<IActionResult> GetAllChats()
    {
        _logger.LogInformation("Вход в метод GetAllChats");

        var rawUserId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        _logger.LogInformation($"Идентификатор пользователя: {rawUserId}");

        var userId = Guid.Parse(rawUserId);
        _logger.LogInformation($"Токен пользователя: {userId}");

        var chats = await _service.GetAllUserChats(userId);

        _logger.LogInformation("Все чаты пользователя получены");
        return Ok(chats);
    }

    [HttpGet]
    [Route("Info")]
    async public Task<IActionResult> GetChatInfo(Guid chatId)
    {
        _logger.LogInformation("Вход в метод GetChatInfo");

        var rawUserId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        _logger.LogInformation($"Идентификатор пользователя: {rawUserId}");

        var userId = Guid.Parse(rawUserId);
        _logger.LogInformation($"Токен пользователя: {userId}");

        var chat = await _service.GetChatInfo(chatId);

        _logger.LogInformation("Данные о беседе получены");

        return Ok(chat);
    }

    [HttpGet]
    [Route("RemoveUser")]
    async public Task<IActionResult> RemoveFromChat(Guid userIdForRemove, Guid chatId)
    {
        _logger.LogInformation("Вход в метод RemoveFromChat");

        var rawUserId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = Guid.Parse(rawUserId);

        try
        {
            await _service.RemoveFromChat(userId, userIdForRemove, chatId);
            _logger.LogInformation("Пользователь успешно удален из беседы");
        } 
        catch(WrongUserInputException ex)
        {
            _logger.LogError(ex, "Ошибка удаления пользователя из беседы");
            return BadRequest(ex.Message);
        }
        catch(Forbidden ex)
        {
            _logger.LogError(ex, "Ошибка удаления пользователя из беседы");
            return BadRequest(ex.Message);
        }
        catch(NotFoundException ex)
        {
            _logger.LogError(ex, "Ошибка удаления пользователя из беседы");
            return NotFound(ex.Message);
        }

        return Ok();
    }
}
