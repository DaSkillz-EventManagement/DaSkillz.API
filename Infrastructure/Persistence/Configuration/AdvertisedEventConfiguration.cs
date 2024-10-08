using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class AdvertisedEventConfiguration : IEntityTypeConfiguration<AdvertisedEvent>
    {
        public void Configure(EntityTypeBuilder<AdvertisedEvent> builder)
        {


            builder.Property(e => e.Id).ValueGeneratedNever();

            // Configure the PurchaserId property
            builder.Property(e => e.UserId)
                .IsRequired(); // Make PurchaserId required

            // Configure the EventId property
            builder.Property(e => e.EventId)
                .IsRequired(); // Make EventId required

            builder.Property(e => e.CreatedDate).IsRequired();
            // Configure the StartDate property
            builder.Property(e => e.StartDate)
                .IsRequired(); // Make StartDate required

            // Configure the EndDate property
            builder.Property(e => e.EndDate)
                .IsRequired(); // Make EndDate required

            builder.Property(e => e.Status)
               .HasMaxLength(10)
               .IsUnicode(false);

            // Configure the PurchasedPrice property
            builder.Property(e => e.PurchasedPrice)
                .HasColumnType("decimal(18,2)") // Set decimal precision for PurchasedPrice
                .IsRequired(); // Make PurchasedPrice required

        
        }
    }
}
