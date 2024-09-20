using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
{
    public void Configure(EntityTypeBuilder<UserAnswer> builder)
    {
        builder.ToTable("UserAnswer");

        builder.Property(ua => ua.UserAnswerId)
            .ValueGeneratedNever();

        builder.Property(ua => ua.UserId)
            .IsRequired();

        builder.Property(ua => ua.QuizId)
            .IsRequired();

        builder.Property(ua => ua.QuestionId)
            .IsRequired();

        builder.Property(ua => ua.AnswerLabel)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasOne(ua => ua.Quiz)
            .WithMany()
            .HasForeignKey(ua => ua.QuizId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(ua => ua.Question)
            .WithMany()
            .HasForeignKey(ua => ua.QuestionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(ua => ua.User)
            .WithMany()
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
