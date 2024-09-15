

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasKey(e => new { e.UserId, e.EventId })
                    .HasName("PK__Feedback__001C802BACDA8C5B");

        builder.ToTable("Feedback");

        builder.Property(e => e.UserId).HasColumnName("UserID");

        builder.Property(e => e.EventId).HasColumnName("EventID");

        builder.Property(e => e.Content).HasColumnType("text");

        builder.Property(e => e.CreatedAt).HasColumnType("datetime");

        builder.HasOne(d => d.Event)
            .WithMany(p => p.Feedbacks)
            .HasForeignKey(d => d.EventId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Feedback__EventI__4E88ABD4");

        builder.HasOne(d => d.User)
            .WithMany(p => p.Feedbacks)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Feedback__UserID__4D94879B");
    }
}
