using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CinemaMagic.AuthService.Entities;
using CinemaMagic.AuthService.Models;
using CinemaMagic.AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CinemaMagic.AuthService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService): ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserRegisterDto userDto)
    {
        return  Ok(await authService.RegisterAsync(userDto));
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(UserAuthDto userDto)
    {
        return Ok(await authService.LoginAsync(userDto));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Admin-Access")]
    public ActionResult<string> AdminAction()
    {
        return Ok("Admin action succeeded");
    }
    
    [Authorize(Roles = "User")]
    [HttpGet("User-Access")]
    public ActionResult<string> UserAction()
    {
        return Ok("Admin action succeeded");
    }

    [HttpPost("Refresh-Token")]
    public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var result = await authService.RefreshTokensAsync(refreshTokenRequestDto);
        
        if (result is null || result.AccessToken is null || result.RefreshToken is null)
            return BadRequest("Invalid Refresh Token");
        
        return Ok(result);
    }
}