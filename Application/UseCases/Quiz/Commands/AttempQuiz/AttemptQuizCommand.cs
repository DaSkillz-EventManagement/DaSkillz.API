using Domain.DTOs.Quiz.Request;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Commands.AttempQuiz;

public class AttemptQuizCommand : IRequest<APIResponse>
{
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    public string TotalTime { get; set; }
    public List<AttempQuizDto> AttempQuizDtos {  get; set; }

    public AttemptQuizCommand(List<AttempQuizDto> attempQuizDtos, Guid userId, Guid quizId, string totalTime)
    {
        AttempQuizDtos = attempQuizDtos;
        UserId = userId;
        QuizId = quizId;
        TotalTime = totalTime;
    }
}
