using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkSentry.Domain.Entities;

namespace ParkSentry.Infrastructure.Configurations;

public class VehicleOwnerConfiguration : IEntityTypeConfiguration<VehicleOwner>
{
    public void Configure(EntityTypeBuilder<VehicleOwner> builder)
    {
        builder.ToTable("vehicle_owners");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("id").UseIdentityColumn();
        builder.Property(v => v.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(v => v.IdCardNo).HasColumnName("id_card_no").HasMaxLength(50).IsRequired();
        builder.Property(v => v.Address).HasColumnName("address");
        builder.Property(v => v.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
        builder.Property(v => v.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");

        builder.HasIndex(v => v.UserId).IsUnique();
        builder.HasIndex(v => v.IdCardNo).IsUnique();

        builder.HasOne(v => v.User)
            .WithOne(u => u.VehicleOwner)
            .HasForeignKey<VehicleOwner>(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
