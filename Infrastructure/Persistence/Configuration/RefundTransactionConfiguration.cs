using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configuration
{
    public class RefundTransactionConfiguration : IEntityTypeConfiguration<RefundTransaction>
    {
        public void Configure(EntityTypeBuilder<RefundTransaction> entity)
        {
            entity.ToTable("RefundTransactions");

            entity.HasKey(r => r.refundId);

            entity.Property(r => r.refundAmount)
                .IsRequired();

            entity.Property(r => r.returnCode)
                .IsRequired();

            entity.Property(r => r.refundAt)
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(r => r.Apptransid)
                .HasMaxLength(100)
                .IsRequired(false);

            entity.Property(r => r.returnMessage)
                .HasMaxLength(300)
                .IsUnicode(true)
                .IsRequired(false);
        }
    }
}
