using Microsoft.EntityFrameworkCore;
using timer.Features.Auth.CurrentUser;
using timer.Features.Auth.Domain;

namespace timer.Database;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
           
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .HasKey(user => user.Id);
        
        
    } 
}