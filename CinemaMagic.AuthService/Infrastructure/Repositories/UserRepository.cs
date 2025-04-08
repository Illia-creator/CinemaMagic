using CinemaMagic.AuthService.Data;
using CinemaMagic.AuthService.Entities;
using CinemaMagic.AuthService.Models;
using CinemaMagic.AuthService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaMagic.AuthService.Infrastruction.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _authContext;

    public UserRepository(AuthDbContext authContext)
    {
        _authContext = authContext;
    }
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _authContext.Users.SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByPhoneAsync(string phoneNumber)
    {
        return await _authContext.Users.SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _authContext.Users.SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddUserAsync(User user)
    {
        var isUserExist = await _authContext.Users.AnyAsync(u => u.Email == user.Email);
        
        if (isUserExist)
            throw new ApplicationException($"User with email {user.Email} already exist");
        
        await _authContext.Users.AddAsync(user);
        await _authContext.SaveChangesAsync();
    }

    public async Task UpdateRefreshTokenAsync(User user)
    {
        _authContext.Users.Update(user);
        await _authContext.SaveChangesAsync();
    }
}