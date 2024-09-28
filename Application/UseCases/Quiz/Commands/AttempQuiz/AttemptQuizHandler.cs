using Application.ResponseMessage;
using AutoMapper;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Commands.AttempQuiz;

public class AttemptQuizHandler : IRequestHandler<AttemptQuizCommand, APIResponse>
{
    #region DI
    private readonly IUserAnswerRepository _userAnswerRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IAnswerRepository _answerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AttemptQuizHandler(IUserAnswerRepository userAnswerRepository, IQuestionRepository questionRepository, 
        IAnswerRepository answerRepository, IUnitOfWork unitOfWork)
    {
        _userAnswerRepository = userAnswerRepository;
        _questionRepository = questionRepository;
        _answerRepository = answerRepository;
        _unitOfWork = unitOfWork;
    }
    #endregion
    public async Task<APIResponse> Handle(AttemptQuizCommand request, CancellationToken cancellationToken)
    {
        bool isAttempted = await _userAnswerRepository.IsAttempted(request.QuizId);
        if (isAttempted)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageEvent.YouAlreadyAttemptedThisQuiz,
                Data = MessageEvent.YouAlreadyAttemptedThisQuiz
            };
        }
        int maxPoint = await _questionRepository.CountQuestion(request.QuizId);
        int userPoint = 0;
        foreach(var attempQuizDto in request.AttempQuizDtos)
        {
            UserAnswer entity = new UserAnswer();
            entity.UserAnswerId = Guid.NewGuid();
            entity.TotalTime = request.TotalTime;
            entity.QuizId = request.QuizId;
            entity.QuestionId = attempQuizDto.QuestionId;
            entity.TotalTime = request.TotalTime;
            entity.UserId = request.UserId; 
            if(attempQuizDto.AnswerId != null)
            {
                entity.AnswerContent = attempQuizDto.AnswerId.ToString();
                Answer userAnswer = await _answerRepository.GetById(attempQuizDto.AnswerId);
                if (userAnswer!.IsCorrectAnswer)
                {
                    userPoint++;
                    entity.AnswerContent = userAnswer.AnswerContent;
                }
            }
            if(attempQuizDto.AnswerContent != null) {
                entity.AnswerContent = attempQuizDto.AnswerContent;
                Question question = await _questionRepository.GetById(attempQuizDto?.QuestionId);
                if (question.CorrectAnswerLabel.Equals(attempQuizDto.AnswerContent, StringComparison.OrdinalIgnoreCase))
                {
                    userPoint++;
                }
            }
            await _userAnswerRepository.Add(entity);
        }
        try
        {
            if(await _unitOfWork.SaveChangesAsync() > 0)
            {
                string finalResult = $"Your final score: {userPoint}/{maxPoint}";
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.CreateSuccesfully,
                    Data = finalResult
                };
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.CreateFailed,
                Data = null
            };

        }catch (Exception ex)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.CreateFailed,
                Data = ex.Message
            };
        }
        
    }
}
