using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("Quiz");
        builder.Property(e => e.QuizId).ValueGeneratedNever();

        builder.Property(e => e.eventId)
            .IsRequired();

        builder.Property(e => e.QuizName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(e => e.QuizDescription)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(e => e.TotalTime)
            .IsRequired().HasMaxLength(250);

        builder.Property(e => e.CreatedBy)
            .IsRequired();

        builder.Property(e => e.CreateAt)
            .IsRequired().HasColumnType("datetime");

        /*builder.HasOne(e => e.Event)
        .WithMany() // 1 event  n quiz
        .HasForeignKey(e => e.eventId)
        .OnDelete(DeleteBehavior.NoAction)
        .HasConstraintName("FK_Event_Quizs");*/

        builder.HasOne(e => e.User)
        .WithMany() // 1 user n quiz
        .HasForeignKey(e => e.CreatedBy)
        .OnDelete(DeleteBehavior.NoAction)
        .HasConstraintName("FK_User_Quizs");

    }
}
