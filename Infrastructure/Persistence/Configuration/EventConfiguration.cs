using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.EndDate).HasColumnType("datetime");
            builder.Property(e => e.EventName).HasMaxLength(250);
            builder.Property(e => e.Fare).HasColumnType("decimal(19, 2)");

            builder.Property(e => e.Image)
                .HasMaxLength(5000)
                .IsUnicode(false);

            builder.Property(e => e.Location).HasMaxLength(500);

            builder.Property(e => e.LocationAddress).HasMaxLength(1000);

            builder.Property(e => e.LocationCoord)
                .HasMaxLength(500)
                .IsUnicode(false);

            builder.Property(e => e.LocationId)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("LocationID");

            builder.Property(e => e.LocationUrl)
                .HasMaxLength(2000)
                .IsUnicode(false);

            builder.Property(e => e.StartDate).HasColumnType("datetime");

            builder.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.Theme)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.UpdatedAt).HasColumnType("datetime");
        }
    }
}
