using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class PriceConfiguration : IEntityTypeConfiguration<Price>
{
    public void Configure(EntityTypeBuilder<Price> builder)
    {
        builder.ToTable("Price");
        builder.HasKey(p => p.PriceId);

        builder.Property(p => p.PriceId)
            .HasColumnType("int")
            .ValueGeneratedOnAdd(); // tự tăng

        builder.Property(p => p.PriceType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.amount)
            .IsRequired();

        builder.Property(p => p.status).IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.unit)
            .HasMaxLength(200);

        builder.Property(p => p.note)
            .HasMaxLength(2000);

        builder.Property(p => p.CreatedAt)
            .IsRequired(true);

        builder.Property(p => p.UpdatedAt)
            .IsRequired(false);

        builder.Property(p => p.CreatedBy)
            .IsRequired(true);

        builder.HasOne(p => p.CreatedByNavigation)
            .WithMany()
            .HasForeignKey(p => p.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
