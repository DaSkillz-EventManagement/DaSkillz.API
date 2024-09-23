using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Quiz.Request;

public class CreateAnswerDto
{
    [Required(ErrorMessage = "AnswerLabel is required!")]
    [MaxLength(150, ErrorMessage = "AnswerLabel max length is 150 characters!")]
    public string AnswerLabel { get; set; } = string.Empty; //label for answer. ex: a,b,c,d


    [Required(ErrorMessage = "Content is required!")]
    [MaxLength(1000, ErrorMessage = "Content max length is 1000 characters!")]
    public string Content { get; set; } = string.Empty;


    public bool IsCorrectAnswer { get; set; } = false;
}
