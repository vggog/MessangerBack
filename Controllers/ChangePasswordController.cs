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

    public ChangePasswordController(IChangePasswordService service) 
        => _service = service;

    [HttpPost]
    [Route("SendEmailCode")]
    public async Task<IActionResult> SendEmailCode([FromBody] SendEmailCodeSchema userData)
    {
        try
        {
            await _service.SendEmailCode(userData);
        }
        catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        return Ok();
    }

    [HttpPost]
    [Route("CompareEmailCodes")]
    public async Task<IActionResult> CompareEmailCodes([FromBody] CompareEmailCodeSchema userData)
    {
        if (await _service.CompareEmailCodes(userData))
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpPost]
    [Route("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordSchema userData)
    {
        try
        {
            await _service.ChangePassword(userData);
        }
        catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch(SamePasswordsException ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }
    
}
