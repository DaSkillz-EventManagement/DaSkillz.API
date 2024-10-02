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
    public class EventStatisticsConfiguration : IEntityTypeConfiguration<EventStatistics>
    {
        public void Configure(EntityTypeBuilder<EventStatistics> builder)
        {
            builder.HasKey(e => e.EventId);
            builder.ToTable("EventStatistics");
            builder.ToView("EventStatistics");
            builder.Metadata.SetIsTableExcludedFromMigrations(true);
        }
    }
}
