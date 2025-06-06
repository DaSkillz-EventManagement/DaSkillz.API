﻿
namespace Domain.DTOs.Quiz.Request;

public class UpdateQuizDto
{
    public Guid QuizId { get; set; }
    public Guid EventId { get; set; }
    public string QuizName { get; set; } = null!;
    public string QuizDescription { get; set; } = null!;
    public string TotalTime { get; set; } = null!;
    public int QuizStatus { get; set; }
    public int AttemptAllow { get; set; } = 1;
    public long? DueDate { get; set; }
}
