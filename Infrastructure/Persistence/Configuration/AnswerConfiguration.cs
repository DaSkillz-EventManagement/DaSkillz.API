using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.ToTable("Answer");

        builder.Property(e => e.AnswerId)
            .ValueGeneratedNever();

        builder.Property(e => e.QuestionId)
            .IsRequired();

        builder.Property(e => e.AnswerLabel)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(e => e.IsCorrectAnswer)
            .IsRequired();

        builder.HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .HasConstraintName("FK_Answer_Question_111022")
            .OnDelete(DeleteBehavior.Cascade); // delete answer when delete question
    }
}
