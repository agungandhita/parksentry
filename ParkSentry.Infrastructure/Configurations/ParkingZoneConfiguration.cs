using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkSentry.Domain.Entities;

namespace ParkSentry.Infrastructure.Configurations;

public class ParkingZoneConfiguration : IEntityTypeConfiguration<ParkingZone>
{
    public void Configure(EntityTypeBuilder<ParkingZone> builder)
    {
        builder.ToTable("parking_zones");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id").UseIdentityColumn();
        builder.Property(p => p.ZoneCode).HasColumnName("zone_code").HasMaxLength(20).IsRequired();
        builder.Property(p => p.ZoneName).HasColumnName("zone_name").HasMaxLength(100).IsRequired();
        builder.Property(p => p.Address).HasColumnName("address");
        builder.Property(p => p.City).HasColumnName("city").HasMaxLength(100);
        builder.Property(p => p.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(p => p.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");

        // parking_zones tidak punya updated_at
        builder.Ignore(p => p.UpdatedAt);

        builder.HasIndex(p => p.ZoneCode).IsUnique();
    }
}
