using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscriptions");

            builder.HasKey(s => s.SubscriptionId);

            builder.Property(s => s.SubscriptionId)
                .IsRequired();

            builder.Property(s => s.UserId)
                .IsRequired();

            builder.Property(s => s.StartDate)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(s => s.EndDate)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(s => s.IsActive)
                .IsRequired();

            builder.HasOne(s => s.User)
                .WithOne(u => u.Subscription)
                .HasForeignKey<Subscription>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Subscription_User");
        }
    }
}
