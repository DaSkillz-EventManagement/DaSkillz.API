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
    public class AdvertisedEventConfiguration : IEntityTypeConfiguration<AdvertisedEvent>
    {
        public void Configure(EntityTypeBuilder<AdvertisedEvent> builder)
        {
           

            // Set the composite primary key (assuming PurchaserId and EventId are the composite keys)
            builder.HasKey(e => new { e.PurchaserId, e.EventId });

            // Configure the PurchaserId property
            builder.Property(e => e.PurchaserId)
                .IsRequired(); // Make PurchaserId required

            // Configure the EventId property
            builder.Property(e => e.EventId)
                .IsRequired(); // Make EventId required

            // Configure the StartDate property
            builder.Property(e => e.StartDate)
                .IsRequired(); // Make StartDate required

            // Configure the EndDate property
            builder.Property(e => e.EndDate)
                .IsRequired(); // Make EndDate required

            // Configure the PurchasedPrice property
            builder.Property(e => e.PurchasedPrice)
                .HasColumnType("decimal(18,2)") // Set decimal precision for PurchasedPrice
                .IsRequired(); // Make PurchasedPrice required
        }
    }
}
