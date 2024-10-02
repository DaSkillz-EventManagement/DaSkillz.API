using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.HasKey(t => t.CertificateID);

        builder.Property(ua => ua.CertificateID).ValueGeneratedOnAdd();

        builder.Property(ua => ua.UserId)
            .IsRequired();

        builder.Property(ua => ua.EventId)
            .IsRequired();

        builder.Property(a => a.IssueDate)
            .HasColumnType("datetime")
            .IsRequired();

        builder.HasOne<Participant>()
               .WithMany(p => p.Certificates)
               .HasForeignKey(c => new { c.UserId, c.EventId }) 
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Certificate_Participant");

        builder.HasOne(d => d.Event)
               .WithMany(p => p.Certificates)
               .HasForeignKey(d => d.EventId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Certificate_Event");
    }
}
