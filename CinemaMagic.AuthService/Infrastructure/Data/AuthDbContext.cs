using CinemaMagic.AuthService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaMagic.AuthService.Data;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}