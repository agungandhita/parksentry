using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkSentry.Domain.Entities;

namespace ParkSentry.Infrastructure.Configurations;

public class OfficerConfiguration : IEntityTypeConfiguration<Officer>
{
    public void Configure(EntityTypeBuilder<Officer> builder)
    {
        builder.ToTable("officers");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasColumnName("id").UseIdentityColumn();
        builder.Property(o => o.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(o => o.BadgeNo).HasColumnName("badge_no").HasMaxLength(20).IsRequired();
        builder.Property(o => o.Department).HasColumnName("department").HasMaxLength(100);
        builder.Property(o => o.Zone).HasColumnName("zone").HasMaxLength(100);
        builder.Property(o => o.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(o => o.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");

        // officers tidak punya kolom updated_at
        builder.Ignore(o => o.UpdatedAt);

        builder.HasIndex(o => o.UserId).IsUnique();
        builder.HasIndex(o => o.BadgeNo).IsUnique();

        builder.HasOne(o => o.User)
            .WithOne(u => u.Officer)
            .HasForeignKey<Officer>(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
