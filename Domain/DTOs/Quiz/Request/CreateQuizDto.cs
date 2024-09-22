
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Quiz.Request;

public class CreateQuizDto
{
    [Required(ErrorMessage = "eventId is required!")]
    public Guid eventId { get; set; }


    [Required(ErrorMessage = "QuizName is required!")]
    [MaxLength(250, ErrorMessage = "QuizName max length is 250 characters!")]
    public string QuizName { get; set; } = null!;


    [Required(ErrorMessage = "QuizDescription is required!")]
    [MaxLength(500, ErrorMessage = "QuizDescription max length is 500 characters!")]
    public string QuizDescription { get; set; } = null!;
}
