namespace Infrastructure.Persistence.Configuration
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> entity)
        {
            entity.ToTable("Transactions");

            entity.HasKey(t => t.Apptransid);

            entity.Property(t => t.Apptransid)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(t => t.Zptransid)
                .IsRequired(false);

            entity.Property(t => t.Amount)
                .IsRequired();

            entity.Property(t => t.Description)
                .HasMaxLength(500)
                .IsUnicode(true)
                .IsRequired(); // Nullable

            entity.Property(t => t.Timestamp)
                .IsRequired();

            entity.Property(t => t.Status)
                .IsRequired();

            entity.Property(t => t.CreatedAt)
                .HasColumnType("datetime")
                .IsRequired();

            entity.HasOne(t => t.RefundTransaction)
                .WithOne(r => r.Transaction)
                .HasForeignKey<RefundTransaction>(r => r.Apptransid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RefundTransaction_Transaction");

            entity.HasOne(t => t.Event)
                .WithMany(e => e.Transactions)
                .HasForeignKey(t => t.EventId)
                .HasConstraintName("FK_Transaction_Event");

            entity.HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .HasConstraintName("FK_Transaction_User");
        }
    }

}
