using EcoStepWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoStepWeb.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Badge> Badges { get; set; }
    public DbSet<DailyStepLog> DailyStepLogs { get; set; }
    public DbSet<ActivityRegistration> ActivityRegistrations { get; set; }
    public DbSet<UserBadge> UserBadges { get; set; }
    
    public AppDbContext()
    {
        
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=../EcoStepWeb.Data/Database/EcoStepDB.db");
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>(_ =>
        {
            _.HasIndex(_ => _.UserName).IsUnique();
            _.HasIndex(_ => _.PasswordHash).IsUnique();
        });

        builder.Entity<Activity>(_ =>
        {
            _.HasIndex(_ => _.Title).IsUnique();
        });

        builder.Entity<ActivityRegistration>()
            .HasIndex(_ => new { _.UserId, _.ActivityId })
            .IsUnique();

        builder.Entity<ActivityRegistration>()
            .HasOne(_ => _.User)
            .WithMany(_ => _.ActivityRegistrations)
            .HasForeignKey(_ => _.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserBadge>()
            .HasIndex(_ => new { _.UserId, _.BadgeId })
            .IsUnique();

        builder.Entity<UserBadge>()
            .HasOne(_ => _.User)
            .WithMany(_ => _.UserBadges)
            .HasForeignKey(_ => _.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(builder);
    }
}
