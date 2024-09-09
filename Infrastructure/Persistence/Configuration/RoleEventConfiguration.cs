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
    public class RoleEventConfiguration : IEntityTypeConfiguration<RoleEvent>
    {
        public void Configure(EntityTypeBuilder<RoleEvent> builder)
        {
            builder.ToTable("RoleEvent");

            builder.Property(e => e.RoleEventId).HasColumnName("RoleEventID");

            builder.Property(e => e.RoleEventName)
                .HasMaxLength(255)
                .IsUnicode(false);
        }
    }
}
