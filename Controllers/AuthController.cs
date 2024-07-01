using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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

    public AuthController(UserManager<IdentityUser<Guid>> manager, IJwtProvider provider) 
    {
        _userManager = manager;
        _jwtProvider = provider;
    }

    [HttpPost]
    [Route("Register")]
    async public Task<IActionResult> Register([FromBody] RegisterUserSchema userData)
    {
        if (userData.Password != userData.RepeatPassword)
        {
            return BadRequest("Password and RepeatPassword must be the same.");
        }

        IdentityUser<Guid> user = new() 
        { 
            UserName = userData.UserName, 
            Email = userData.Email 
        };
        IdentityResult result = await _userManager.CreateAsync(user, userData.Password);
    
        if (!result.Succeeded) return BadRequest(result.Errors);
    
        return Ok();
    }

    [HttpPost]
    [Route("Login")]
    [ProducesResponseType(typeof(LoginUserResponce), (int)HttpStatusCode.OK)]
    async public Task<IActionResult> Login([FromBody] LoginUserSchema userData)
    {
        IdentityUser<Guid> user = await _userManager.FindByNameAsync(userData.UserName);
        
        if (user == null) return NotFound("Пользователь с таким username не найден");

        if (!await _userManager.CheckPasswordAsync(user, userData.Password))
        {
            return BadRequest("Пароль не верный");
        }

        LoginUserResponce responce = new() 
        { 
            AccessToken = _jwtProvider.GenerateTokenByUserId(user.Id.ToString())
        };

        return Ok(responce);
    }
}
