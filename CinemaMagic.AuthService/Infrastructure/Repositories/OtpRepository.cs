using CinemaMagic.AuthService.Data;
using CinemaMagic.AuthService.Entities;
using CinemaMagic.AuthService.Models;
using CinemaMagic.AuthService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaMagic.AuthService.Infrastruction.Repositories;

public class OtpRepository : IOtpRepository
{
    private readonly AuthDbContext _authContext;

    public OtpRepository(AuthDbContext authContext)
    {
        _authContext = authContext;
    }
    public async Task<bool> IsPhoneExists(string phoneNumber)
    {
        return await _authContext.PhoneCodes.AnyAsync(x => x.Phone == phoneNumber) ? true : false;
    }

    public async Task<bool> IsCodeValid(PhoneNumberVerificationDto verificationDto)
    {
        var verificationCode = await _authContext.PhoneCodes.FirstOrDefaultAsync(x =>
            x.Phone == verificationDto.PhoneNumber);
        
        if(verificationCode == null)
            throw new Exception("Phone number not found");
        
        if(verificationCode.Code != verificationDto.Code || verificationCode.IsActive == false)
            throw new Exception("Code is not valid");
        
        verificationCode.IsActive = false;
        _authContext.PhoneCodes.Update(verificationCode);
        await _authContext.SaveChangesAsync();

        return true;
    }

    public async Task SaveOtp(PhoneCode phoneCode)
    {
        await _authContext.PhoneCodes.AddAsync(phoneCode);
        await _authContext.SaveChangesAsync();
    }
    
}