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
    public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            // Set the primary key
            builder.HasKey(ad => ad.EventId);

            // Configure properties
            builder.Property(ad => ad.StartDate)
                .IsRequired();

            builder.Property(ad => ad.EndDate)
                .IsRequired();
        }
    }
}
