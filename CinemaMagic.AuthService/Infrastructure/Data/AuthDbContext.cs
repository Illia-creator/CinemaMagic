using CinemaMagic.AuthService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaMagic.AuthService.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<PhoneCode> PhoneCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();

        modelBuilder.Entity<PhoneCode>()
            .HasIndex(p => p.Phone)
            .IsUnique();
    }
}