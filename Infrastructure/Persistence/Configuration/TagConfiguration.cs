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
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tag");

            builder.Property(e => e.TagId).HasColumnName("TagID");

            builder.Property(e => e.TagName).HasMaxLength(255);

            builder.HasMany(d => d.Events)
                .WithMany(p => p.Tags)
                .UsingEntity<Dictionary<string, object>>(
                    "EventTag",
                    l => l.HasOne<Event>().WithMany().HasForeignKey("EventId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EventTag__EventI__4316F928"),
                    r => r.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EventTag__TagID__440B1D61"),
                    j =>
                    {
                        j.HasKey("TagId", "EventId").HasName("PK__EventTag__72E8B6CB39612262");

                        j.ToTable("EventTag");

                        j.IndexerProperty<int>("TagId").HasColumnName("TagID");

                        j.IndexerProperty<Guid>("EventId").HasColumnName("EventID");
                    });
        }
    }
}
