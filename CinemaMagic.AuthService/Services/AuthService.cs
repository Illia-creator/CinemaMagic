using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CinemaMagic.AuthService.Data;
using CinemaMagic.AuthService.Entities;
using CinemaMagic.AuthService.Models;
using CinemaMagic.AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CinemaMagic.AuthService.Services;

public class AuthService(AuthDbContext context, IConfiguration configuration) : IAuthService
{
   public async Task<TokenResponseDto?> LoginAsync(UserAuthDto userDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == userDto.PhoneNumber); 
        if (!user.PhoneNumber.Equals(userDto.PhoneNumber))
            throw new ApplicationException("User does not match");
        
        if(new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, userDto.Password) != PasswordVerificationResult.Success)
            throw new ApplicationException("Wrong password");

        return await CreateTokenResponse(user);
    }

    public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var user = await ValidateRefreshTokenAsync(refreshTokenRequestDto.UserId, refreshTokenRequestDto.RefreshToken);
        
        if (user is null)
            return null;
        
        return await CreateTokenResponse(user);
    }

    public async Task<User?> RegisterAsync(UserRegisterDto userDto)
    {
        if (await context.Users.AnyAsync(x => x.PhoneNumber == userDto.PhoneNumber))
            return null;

        var newUser = new User()
        {
            PhoneNumber = userDto.PhoneNumber,
            Email = userDto.Email,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Role = userDto.Role
        };
        
        newUser.Password = new PasswordHasher<User>()
            .HashPassword(newUser, userDto.Password);
        
        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();
        
        return newUser;
    }

    private async Task<string> CreateRefreshTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.Now.AddDays(7);
        
        await context.SaveChangesAsync();
        return refreshToken;
    }
    
    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
         
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
         
        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("AppSettings:Issuer"),
            audience: configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );
         
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        
        rng.GetBytes(randomNumber);
        
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        
        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            return null;
        
        return user;
    }

    private async Task<TokenResponseDto?> CreateTokenResponse(User? user)
    {
        return new TokenResponseDto()
        {
            AccessToken = CreateToken(user),
            RefreshToken = await CreateRefreshTokenAsync(user),
        };
    }
}