using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Identity;

using MessangerBack.Schemas;
using MessangerBack.Responces;
using MessangerBack.Utils;


namespace MessangerBack.Controllers;

[Route("api")]
[ApiController]
public class AuthController : ControllerBase
{
    UserManager<IdentityUser<Guid>> _userManager;
    IJwtProvider _jwtProvider;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<IdentityUser<Guid>> manager, 
        IJwtProvider provider, 
        ILogger<AuthController> logger) 
    {
        _logger = logger;
        _userManager = manager;
        _jwtProvider = provider;
    }

    [HttpPost]
    [Route("Register")]
    async public Task<IActionResult> Register([FromBody] RegisterUserSchema userData)
    {
        _logger.LogInformation("Вызван контроллер регистрации. Данные пользователя: {@UserData}", userData);
        if (userData.Password != userData.RepeatPassword)
        {
            _logger.LogWarning("Пароли не должны совпадать. Данные пользователя: {UserName}", userData.UserName);
            return BadRequest("Password and RepeatPassword must be the same.");
        }

        IdentityUser<Guid> user = new() 
        { 
            UserName = userData.UserName, 
            Email = userData.Email 
        };
        IdentityResult result = await _userManager.CreateAsync(user, userData.Password);
    
        if (!result.Succeeded)
        {
            _logger.LogError("Ошибка создания пользователя. Ошибки: {@Errors}", result.Errors); 
            return BadRequest(result.Errors);
        } 

        _logger.LogInformation("Пользователь \"{UserName}\" успешно зарегистрирован.", userData.UserName);
        return Ok();
    }

    [HttpPost]
    [Route("Login")]
    [ProducesResponseType(typeof(LoginUserResponce), (int)HttpStatusCode.OK)]
    async public Task<IActionResult> Login([FromBody] LoginUserSchema userData)
    {
        _logger.LogInformation("Вызван контроллер входа. Данные пользователя: {@UserData}", userData);
        IdentityUser<Guid> user = await _userManager.FindByNameAsync(userData.UserName);
        
        if (user == null)
        {
            _logger.LogWarning("Пользователь \"{UserName}\" не найден", userData.UserName);
            return NotFound("Пользователь с таким username не найден"); 
        } 
        else
        {
            _logger.LogInformation("Пользователь \"{UserName}\" найден", userData.UserName);
        }

        if (!await _userManager.CheckPasswordAsync(user, userData.Password))
        {
            _logger.LogWarning("Пароль не верный для пользователя \"{UserName}\"", userData.UserName);
            return BadRequest("Пароль не верный");
        }
        else
        {
            _logger.LogInformation("Пароль для пользователя \"{UserName}\" верный", userData.UserName);
        }

        LoginUserResponce responce = new() 
        { 
            AccessToken = _jwtProvider.GenerateTokenByUserId(user.Id.ToString())
        };

        _logger.LogInformation("Token для пользователя \"{UserName}\" сгенерирован", userData.UserName);

        return Ok(responce);
    }
}
