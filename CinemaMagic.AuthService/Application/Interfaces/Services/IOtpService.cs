using CinemaMagic.AuthService.Models;

namespace CinemaMagic.AuthService.Services.Interfaces;

public interface IOtpService
{
    Task<bool> VerifyPhoneNumberAsync(PhoneNumberVerificationDto phoneNumber);
    Task GeneratePhoneCodeAsync(string phoneNumber);
    Task<TokenResponseDto> LoginByPhoneCodeAsync(PhoneNumberVerificationDto phoneNumber); 
}