using CinemaMagic.AuthService.Entities;
using CinemaMagic.AuthService.Models;

namespace CinemaMagic.AuthService.Services.Interfaces;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserRegisterDto userAuthDto);
    Task<TokenResponseDto?> LoginAsync(UserAuthDto userAuthDto);
    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto refreshTokenRequestDto);
}