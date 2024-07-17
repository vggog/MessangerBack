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
    private readonly ILogger<MessageController> _logger;
    public MessageController(IMessageService service, ILogger<MessageController> logger) 
    { 
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    async public Task<IActionResult> GetChatMessages(Guid chatId)
    {
        _logger.LogInformation("Вход в метод GetChatMessages");
        
        var rawUserId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var userId = Guid.Parse(rawUserId);

        List<MessageResponce> responce;
        try
        {
            _logger.LogInformation("Получение сообщений чата начато");
            responce = await _service.GetChatMessages(chatId, userId);
            _logger.LogInformation("Сообщения чата получены успешно");
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Ошибка: сообщения чата не найдены");
            return NotFound(ex.Message);
        }

        return Ok(responce);
    }
}
