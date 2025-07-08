using CinemaMagic.AuthService.Models;
using CinemaMagic.AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMagic.AuthService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OtpController(IOtpService userService): ControllerBase
{
    [HttpPost("verify-phone-number")]
    public async Task<ActionResult<bool>> VerifyPhoneNumber(PhoneNumberVerificationDto phoneDto)
    {
        return Ok(await userService.VerifyPhoneNumberAsync(phoneDto));
    }
    
    [HttpPost("generate-phone-code")]
    public async Task<ActionResult<bool>> GeneratePhoneCode(string phone)
    {
        await userService.GeneratePhoneCodeAsync(phone);
        return Ok();
    }
    
    [HttpPost("login-phone-code")]
    public async Task<ActionResult<bool>> LoginByPhoneCode(PhoneNumberVerificationDto phoneDto)
    {
        return Ok(await userService.LoginByPhoneCodeAsync(phoneDto));
    }
}