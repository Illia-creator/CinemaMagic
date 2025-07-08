using CinemaMagic.AuthService.Entities;
using CinemaMagic.AuthService.Models;
using CinemaMagic.AuthService.Services.Interfaces;

namespace CinemaMagic.AuthService.Services;

public class OtpService : IOtpService
{
    private readonly IConfiguration _configuration;
    private readonly IOtpRepository _optRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public OtpService(IConfiguration configuration, IOtpRepository optRepository, IUserRepository userRepository, ITokenService tokenService)
    {
        _configuration = configuration;
        _optRepository = optRepository;
        _userRepository = userRepository;
        _tokenService = tokenService;
    }
    

    public async Task<bool> VerifyPhoneNumberAsync(PhoneNumberVerificationDto phoneVerification)
    {
        if (await _optRepository.IsCodeValid(phoneVerification) == false)
            throw new ApplicationException("Phone validation failed");

        return true;
    }

    public async Task GeneratePhoneCodeAsync(string phoneNumber)
    {
        Random generator = new Random();
        var generatedCode = generator.Next(0, 99999).ToString("D5");

        var newPhoneCode = new PhoneCode
        {
            Phone = phoneNumber,
            Code = generatedCode
        };
        await _optRepository.SaveOtp(newPhoneCode);
    }

    public async Task<TokenResponseDto> LoginByPhoneCodeAsync(PhoneNumberVerificationDto phoneNumber)
    {
        if (await _optRepository.IsCodeValid(phoneNumber) == false)
            throw new ApplicationException("Phone validation failed");
        
        var user = await _userRepository.GetUserByPhoneAsync(phoneNumber.PhoneNumber);
        
        if (!user.PhoneNumber.Equals(phoneNumber.PhoneNumber))
            throw new ApplicationException("User does not match");
        
        return await _tokenService.CreateTokenResponse(user);
    }
}