using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkSentry.Domain.Entities;
using ParkSentry.Domain.Enums;

namespace ParkSentry.Infrastructure.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("id").UseIdentityColumn();
        builder.Property(v => v.OwnerId).HasColumnName("owner_id").IsRequired();
        builder.Property(v => v.PlateNumber).HasColumnName("plate_number").HasMaxLength(20).IsRequired();
        builder.Property(v => v.Brand).HasColumnName("brand").HasMaxLength(50);
        builder.Property(v => v.Model).HasColumnName("model").HasMaxLength(50);
        builder.Property(v => v.Color).HasColumnName("color").HasMaxLength(30);
        builder.Property(v => v.Year).HasColumnName("year");
        builder.Property(v => v.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(v => v.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");

        // vehicle_type disimpan sebagai string lowercase di PostgreSQL
        builder.Property(v => v.VehicleType)
            .HasColumnName("vehicle_type")
            .HasMaxLength(20)
            .HasConversion(
                v => v.ToString().ToLower(),
                v => Enum.Parse<VehicleType>(v, true)
            )
            .IsRequired();

        // vehicles tidak punya kolom updated_at
        builder.Ignore(v => v.UpdatedAt);

        builder.HasIndex(v => v.PlateNumber).IsUnique();

        builder.HasOne(v => v.Owner)
            .WithMany(o => o.Vehicles)
            .HasForeignKey(v => v.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
