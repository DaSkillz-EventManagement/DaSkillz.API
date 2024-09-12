using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.HasKey(e => new { e.UserId, e.EventId });


            builder.ToTable("Participant");

            builder.Property(e => e.UserId).HasColumnName("UserID");

            builder.Property(e => e.EventId).HasColumnName("EventID");

            builder.Property(e => e.CheckedIn).HasColumnType("datetime");

            builder.Property(e => e.CreatedAt).HasColumnType("datetime");

            builder.Property(e => e.RoleEventId).HasColumnName("RoleEventID");

            builder.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.HasOne(d => d.Event)
                .WithMany(p => p.Participants)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            builder.HasOne(d => d.RoleEvent)
                .WithMany(p => p.Participants)
                .HasForeignKey(d => d.RoleEventId);


            builder.HasOne(d => d.User)
                .WithMany(p => p.Participants)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
