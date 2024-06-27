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
    {
        _service = service;
    }

    [HttpPost]
    [Route("SendEmailCode")]
    public async Task<IActionResult> SendEmailCode([FromBody] SendEmailCodeSchema userData)
    {
        await _service.SendEmailCode(userData);
        return Ok();
    }

    [HttpPost]
    [Route("CompareEmailCodes")]
    public async Task<IActionResult> CompareEmailCodes([FromBody] CompareEmailCodeSchema userData)
    {
        bool result = await _service.CompareEmailCodes(userData);
        if (!result)
        {
            return BadRequest();
        }
        return Ok();
    }

    [HttpPost]
    [Route("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordSchema userData)
    {
        await _service.ChangePassword(userData);
        return Ok();
    }
    
}
