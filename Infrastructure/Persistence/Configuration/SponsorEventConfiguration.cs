using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class SponsorEventConfiguration : IEntityTypeConfiguration<SponsorEvent>
    {
        public void Configure(EntityTypeBuilder<SponsorEvent> builder)
        {
            builder.HasKey(se => new { se.EventId, se.UserId });

            builder.ToTable("SponsorEvent");

            builder.Property(e => e.Amount).HasColumnType("decimal(19, 2)");

            builder.Property(e => e.CreatedAt).HasColumnType("datetime");

            builder.Property(e => e.EventId).HasColumnName("EventID");

            //entity.Property(e => e.SponsorMethodId).HasColumnName("SponsorMethodID");

            builder.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.Message).HasMaxLength(200);

            builder.Property(e => e.UpdatedAt).HasColumnType("datetime");

            builder.Property(e => e.UserId).HasColumnName("UserID");

            /*builder.HasOne(d => d.Event)
                .WithMany()
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__SponsorEv__Event__5FB337D6");*/

            builder.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SponsorEv__UserI__60A75C0F");
        }
    }
}
