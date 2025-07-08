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

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public UserService(IConfiguration configuration, IUserRepository userRepository, ITokenService tokenService)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _tokenService = tokenService;
    }
    
   public async Task<TokenResponseDto?> LoginAsync(UserAuthDto userDto)
    {
        var user = await _userRepository.GetUserByPhoneAsync(userDto.PhoneNumber);
        
        if (!user.PhoneNumber.Equals(userDto.PhoneNumber))
            throw new ApplicationException("User does not match");
        
        if(new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, userDto.Password) != PasswordVerificationResult.Success)
            throw new ApplicationException("Wrong password");

        return await _tokenService.CreateTokenResponse(user);
    }

    public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        
        var user = await _userRepository.GetUserByIdAsync(refreshTokenRequestDto.UserId);
        
        var isValid = await _tokenService.ValidateRefreshTokenAsync(user, refreshTokenRequestDto.RefreshToken);
        
        if (!isValid)
            return null;
        
        return await _tokenService.CreateTokenResponse(user);
    }

    public async Task<User?> RegisterAsync(UserRegisterDto userDto)
    {
        if (await _userRepository.GetUserByPhoneAsync(userDto.PhoneNumber) is not null)
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
        
        await _userRepository.AddUserAsync(newUser);
        
        return newUser;
    }
}