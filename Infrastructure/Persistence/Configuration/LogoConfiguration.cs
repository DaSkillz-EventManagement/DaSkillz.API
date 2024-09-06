using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configuration
{
    public class LogoConfiguration : IEntityTypeConfiguration<Logo>
    {
        public void Configure(EntityTypeBuilder<Logo> builder)
        {
            builder.ToTable("Logo");

            builder.Property(e => e.LogoId).HasColumnName("LogoID");

            builder.Property(e => e.LogoUrl)
                .HasMaxLength(1000)
                .IsUnicode(false);

            builder.Property(e => e.SponsorBrand)
                .HasMaxLength(500)
                .IsUnicode(false);

            builder.HasMany(d => d.Events)
                .WithMany(p => p.Logos)
                .UsingEntity<Dictionary<string, object>>(
                    "EventLogo",
                    l => l.HasOne<Event>().WithMany().HasForeignKey("EventId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EventLogo__Event__5CD6CB2B"),
                    r => r.HasOne<Logo>().WithMany().HasForeignKey("LogoId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EventLogo__LogoI__5DCAEF64"),
                    j =>
                    {
                        j.HasKey("LogoId", "EventId").HasName("PK__EventLog__D1B4592A90BF4813");

                        j.ToTable("EventLogo");

                        j.IndexerProperty<int>("LogoId").HasColumnName("LogoID");

                        j.IndexerProperty<Guid>("EventId").HasColumnName("EventID");
                    });
        }
    }
}
