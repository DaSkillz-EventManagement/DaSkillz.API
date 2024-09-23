using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Question");

        builder.Property(e => e.QuestionId)
            .ValueGeneratedNever();

        builder.Property(e => e.QuizId)
            .IsRequired();

        builder.Property(e => e.QuestionName)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.IsMultipleAnswers)
            .IsRequired();

        builder.Property(e => e.IsQuestionAnswered)
            .IsRequired();

        builder.Property(e => e.ShowAnswerAfterChoosing)
            .IsRequired();

        builder.Property(e => e.CorrectAnswerLabel)
            .IsRequired();

        builder.HasOne(q => q.Quiz)
            .WithMany(qz => qz.Question) // 1 quiz n question
            .HasForeignKey(q => q.QuizId)
            .HasConstraintName("FK_Quiz_Questions")
            .OnDelete(DeleteBehavior.NoAction); // delete when delete quiz
    }
}
