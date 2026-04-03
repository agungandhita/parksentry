using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkSentry.Domain.Entities;

namespace ParkSentry.Infrastructure.Configurations;

public class ViolationTypeConfiguration : IEntityTypeConfiguration<ViolationType>
{
    public void Configure(EntityTypeBuilder<ViolationType> builder)
    {
        builder.ToTable("violation_types");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("id").UseIdentityColumn();
        builder.Property(v => v.Code).HasColumnName("code").HasMaxLength(20).IsRequired();
        builder.Property(v => v.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(v => v.Description).HasColumnName("description");
        builder.Property(v => v.FineAmount).HasColumnName("fine_amount").HasColumnType("numeric(15,2)").HasDefaultValue(0m);
        builder.Property(v => v.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");

        // violation_types tidak punya updated_at
        builder.Ignore(v => v.UpdatedAt);

        builder.HasIndex(v => v.Code).IsUnique();
    }
}
