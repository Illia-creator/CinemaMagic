using CinemaMagic.AuthService.Entities;
using CinemaMagic.AuthService.Models;

namespace CinemaMagic.AuthService.Services.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<User?> GetUserByPhoneAsync(string phoneNumber);
    public Task<User?> GetUserByIdAsync(Guid id);
    
    public Task AddUserAsync(User user);
    
    public Task UpdateRefreshTokenAsync(User user);
    
}