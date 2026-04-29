using Microsoft.EntityFrameworkCore;
using timer.Features.Auth.CurrentUser;
using timer.Features.Auth.Domain;
using timer.Features.Timer.Run.Domain;
using timer.Features.Timer.Settings.Domain;

namespace timer.Database;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<SettingsEntity> TimerSettings => Set<SettingsEntity>();
    public DbSet<RunEntity> RunEntities => Set<RunEntity>();
    
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
           
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .HasKey(user => user.Id);

        modelBuilder.Entity<SettingsEntity>()
            .HasKey(setting => setting.Id);

        modelBuilder.Entity<RunEntity>()
            .HasKey(run => run.Id);
        
        
    } 
}