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
    public class SearchHistoryConfiguration : IEntityTypeConfiguration<SearchHistory>
    {
        public void Configure(EntityTypeBuilder<SearchHistory> builder)
        {
            builder.HasNoKey();

            // Configure EventName property
            builder.Property(e => e.EventName)
                .HasMaxLength(100)   // Define a max length for the column
                .IsRequired(false);  // Allow null values (since it's nullable)

            // Configure Location property
            builder.Property(e => e.Location)
                .HasMaxLength(100)   // Define a max length for the column
                .IsRequired(false);  // Allow null values (since it's nullable)

            // Configure Hashtag property
            builder.Property(e => e.Hashtag)
                .HasMaxLength(50)    // Define a max length for the column
                .IsRequired(false);  // Allow null values (since it's nullable)

            // Configure CreatedDate property
            builder.Property(e => e.CreatedDate)
                .IsRequired();       // Make sure this column is required
        }

    }
}
