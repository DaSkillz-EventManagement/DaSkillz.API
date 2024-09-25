using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            // Configure Id property to auto-increment
            builder.HasKey(c => c.Id);  // Set Id as the primary key

            builder.Property(c => c.Id)
                   .IsRequired();  // Make sure Id is required

            // Configure ExpiredDate property
            builder.Property(c => c.ExpiredDate)
                   .IsRequired();  // Make it required

            // Configure NOAttempts property
            builder.Property(c => c.NOAttempts)
                   .IsRequired()
                   .HasDefaultValue(1)  // Default value is 1 attempt
                   .HasComment("Number of attempts allowed for the coupon");  // Optional comment

            // Configure DiscountType property
            builder.Property(c => c.DiscountType)
                   .IsRequired()
                   .HasMaxLength(10)  // Set a max length to limit input, e.g., 'Money' or 'Percent'
                   .HasConversion<string>() // Ensure the type is stored as a string
                   .HasComment("Type of discount: 'Money' or 'Percent'");  // Optional comment

            // Configure Value property
            builder.Property(c => c.Value)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)")  // Ensure the value has 2 decimal precision
                   .HasComment("Discount value, depends on DiscountType (money or percentage)");  // Optional comment
        }
    }
}
