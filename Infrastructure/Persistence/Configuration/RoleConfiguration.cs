using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {

            builder.ToTable("Role");

            builder.Property(e => e.RoleId)
                .HasColumnName("RoleID");

            builder.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
