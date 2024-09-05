using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken");

            builder.Property(e => e.RefreshTokenId).HasColumnName("RefreshTokenID");

            builder.Property(e => e.CreatedAt).HasColumnType("datetime");

            builder.Property(e => e.ExpireAt).HasColumnType("datetime");

            builder.Property(e => e.Token).HasMaxLength(300);

            builder.Property(e => e.UserId).HasColumnName("UserID");

            builder.HasOne(d => d.User)
                .WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__RefreshTo__UserI__5535A963");
        }
    }
}
