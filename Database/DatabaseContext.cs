using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database;


public sealed class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
    
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Animal> Animals { get; set; } = null!;
    public DbSet<AType> Types { get; set; } = null!;
    public DbSet<Area> Areas { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<VisitedLocation> AnimalVisitedLocations { get; set; } = null!;
    public DbSet<AnimalType> AnimalTypes { get; set; } = null!;
    public DbSet<AreaLocation> AreaLocations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseIdentityColumns();
        modelBuilder.Entity<Account>()
            .Property(x => x.Role)
            .HasConversion<string>();
        modelBuilder.Entity<Animal>()
            .Property(x => x.LifeStatus)
            .HasConversion<string>();
        modelBuilder.Entity<Animal>()
            .Property(x => x.Gender)
            .HasConversion<string>();
    }
}