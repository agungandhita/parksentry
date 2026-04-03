using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkSentry.Domain.Entities;
using ParkSentry.Domain.Enums;

namespace ParkSentry.Infrastructure.Configurations;

public class ViolationTicketConfiguration : IEntityTypeConfiguration<ViolationTicket>
{
    public void Configure(EntityTypeBuilder<ViolationTicket> builder)
    {
        builder.ToTable("violation_tickets");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id").UseIdentityColumn();
        builder.Property(t => t.TicketNo).HasColumnName("ticket_no").HasMaxLength(50).IsRequired();
        builder.Property(t => t.OfficerId).HasColumnName("officer_id").IsRequired();
        builder.Property(t => t.VehicleId).HasColumnName("vehicle_id").IsRequired();
        builder.Property(t => t.ViolationTypeId).HasColumnName("violation_type_id").IsRequired();
        builder.Property(t => t.ZoneId).HasColumnName("zone_id").IsRequired();
        builder.Property(t => t.ViolationDate).HasColumnName("violation_date").IsRequired();
        builder.Property(t => t.FineAmount).HasColumnName("fine_amount").HasColumnType("numeric(15,2)").IsRequired();
        builder.Property(t => t.DueDate).HasColumnName("due_date").IsRequired();
        builder.Property(t => t.LocationDetail).HasColumnName("location_detail");
        builder.Property(t => t.PhotoEvidence).HasColumnName("photo_evidence").HasMaxLength(255);
        builder.Property(t => t.Notes).HasColumnName("notes");
        builder.Property(t => t.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
        builder.Property(t => t.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");

        // status disimpan sebagai string lowercase di PostgreSQL
        builder.Property(t => t.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .HasDefaultValue(TicketStatus.Unpaid)
            .HasConversion(
                s => s.ToString().ToLower(),
                s => Enum.Parse<TicketStatus>(s, true)
            );

        builder.HasIndex(t => t.TicketNo).IsUnique();

        builder.HasOne(t => t.Officer)
            .WithMany(o => o.IssuedTickets)
            .HasForeignKey(t => t.OfficerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Vehicle)
            .WithMany(v => v.Tickets)
            .HasForeignKey(t => t.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ViolationType)
            .WithMany(vt => vt.Tickets)
            .HasForeignKey(t => t.ViolationTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Zone)
            .WithMany(z => z.Tickets)
            .HasForeignKey(t => t.ZoneId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
