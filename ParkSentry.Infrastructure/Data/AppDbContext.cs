using Microsoft.EntityFrameworkCore;
using ParkSentry.Domain.Entities;
using ParkSentry.Infrastructure.Configurations;

namespace ParkSentry.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSet untuk setiap entity
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Officer> Officers => Set<Officer>();
    public DbSet<VehicleOwner> VehicleOwners => Set<VehicleOwner>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<ViolationType> ViolationTypes => Set<ViolationType>();
    public DbSet<ParkingZone> ParkingZones => Set<ParkingZone>();
    public DbSet<ViolationTicket> ViolationTickets => Set<ViolationTicket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply semua konfigurasi dari folder Configurations
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new OfficerConfiguration());
        modelBuilder.ApplyConfiguration(new VehicleOwnerConfiguration());
        modelBuilder.ApplyConfiguration(new VehicleConfiguration());
        modelBuilder.ApplyConfiguration(new ViolationTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ParkingZoneConfiguration());
        modelBuilder.ApplyConfiguration(new ViolationTicketConfiguration());

        // Set default schema ke public (PostgreSQL)
        modelBuilder.HasDefaultSchema("public");
    }
}
