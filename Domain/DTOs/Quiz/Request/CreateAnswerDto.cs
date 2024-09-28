using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Quiz.Request;

public class CreateAnswerDto
{
    
    [Required(ErrorMessage = "Content is required!")]
    [MaxLength(1000, ErrorMessage = "Content max length is 1000 characters!")]
    public string Content { get; set; } = string.Empty;
    public bool IsCorrectAnswer { get; set; } = false;
}
