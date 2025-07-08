using CinemaMagic.AuthService.Entities;
using CinemaMagic.AuthService.Models;

namespace CinemaMagic.AuthService.Services.Interfaces;

public interface IOtpRepository
{
    public Task<bool> IsPhoneExists(string phoneNumber);
    public Task<bool> IsCodeValid(PhoneNumberVerificationDto phoneDto);
    public Task SaveOtp(PhoneCode phoneCode);
}