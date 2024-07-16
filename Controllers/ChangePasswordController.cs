using MessangerBack.Exceptions;
using MessangerBack.Schemas;
using MessangerBack.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessangerBack.Controllers;


[Route("api")]
[ApiController]
public class ChangePasswordController : ControllerBase
{
    IChangePasswordService _service;
    private readonly ILogger<ChangePasswordController> _logger;

    public ChangePasswordController(IChangePasswordService service, ILogger<ChangePasswordController> logger) 
    { 
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    [Route("SendEmailCode")]
    public async Task<IActionResult> SendEmailCode([FromBody] SendEmailCodeSchema userData)
    {
        _logger.LogInformation("Отправка кода для смена пароля на данную почту: {Email}", userData.Email);
        try
        {
            await _service.SendEmailCode(userData);
            _logger.LogInformation("Отправка кода на данный электронный адрес прошла успешно: {Email}", userData.Email);
        }
        catch(NotFoundException ex)
        {
            _logger.LogWarning("Такого Email не существует: {Email}, Ошибка: {Message}", userData.Email, ex.Message);
            return NotFound(ex.Message);
        }
        catch(FormatException ex)
        {
            _logger.LogWarning("Некорректный Email: {Email}, Ошибка: {Message}", userData.Email, ex.Message);
            return BadRequest(ex.Message);
        }
        return Ok();
    }

    [HttpPost]
    [Route("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordSchema userData)
    {
        _logger.LogInformation("Начинается процесс смены пароля для пользователя с email: {Email}", userData.Email);
        try
        {
            await _service.ChangePassword(userData);
            _logger.LogInformation("Процесс смены пароля для пользователя с email: {Email} был успешно завершен", userData.Email);
        }
        catch(NotFoundException ex)
        {
            _logger.LogWarning("Пользователь с email: {Email} не найден. Ошибка: {Message}", userData.Email, ex.Message);
            return NotFound(ex.Message);
        }
        catch(WrongUserInputException ex)
        {
            _logger.LogWarning("Некорректный ввод пользователем. Ошибка: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }

        return Ok();
    }
}
