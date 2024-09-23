using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");

            builder.Property(e => e.Avatar)
                .HasMaxLength(1000)
                .IsUnicode(false);

            builder.Property(e => e.CreatedAt)
                .HasColumnType("datetime");

            builder.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.FullName)
                .HasMaxLength(255);

            builder.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);

            builder.Property(e => e.RoleId)
                .HasColumnName("RoleID");

            builder.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.UpdatedAt).HasColumnType("datetime");

            builder.Property(e => e.IsPremiumUser)
                .HasDefaultValue(false);


            builder.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__RoleID__398D8EEE");
        }
    }
}
