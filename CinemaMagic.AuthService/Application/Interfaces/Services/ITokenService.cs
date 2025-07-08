using CinemaMagic.AuthService.Entities;
using CinemaMagic.AuthService.Models;

namespace CinemaMagic.AuthService.Services.Interfaces;

public interface ITokenService
{
    Task<bool> ValidateRefreshTokenAsync(User user, string refreshToken);
    Task<TokenResponseDto?> CreateTokenResponse(User? user);
}