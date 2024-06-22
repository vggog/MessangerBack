using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net;
using MessangerBack.Models;
using MessangerBack.Repositories;
using MessangerBack.Schemas;
using MessangerBack.Services;
using MessangerBack.Exceptions;
using MessangerBack.Responces;


namespace MessangerBack.Controllers;

[Route("api")]
[ApiController]
public class AuthController : ControllerBase
{
    IAuthService _service;

    public AuthController(IAuthService service) 
    {
        _service = service;
    }

    [HttpPost]
    [Route("Register")]
    async public Task<IActionResult> Post([FromBody] RegisterUserSchema userData)
    {
        if (userData.Password != userData.RepeatPassword)
        {
            return BadRequest("Password and RepeatPassword must be the same.");
        }

        await _service.RegisterUser(userData);
        return Ok();
    }

    [HttpPost]
    [Route("Login")]
    [ProducesResponseType(typeof(LoginUserResponce), (int)HttpStatusCode.OK)]
    async public Task<IActionResult> Login([FromBody] LoginUserSchema userData)
    {
        string token;

        try
        {
            token = await _service.LoginUser(userData);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (PasswordIsIncorrectException ex)
        {
            return BadRequest(ex.Message);
        }

        LoginUserResponce responce = new() { AccessToken = token };

        return Ok(responce);
    }
}
