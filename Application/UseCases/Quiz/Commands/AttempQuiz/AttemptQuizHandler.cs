﻿using Application.Abstractions.Caching;
using Application.ResponseMessage;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Medallion.Threading;
using MediatR;
using System.Net;

namespace Application.UseCases.Quizs.Commands.AttempQuiz;

public class AttemptQuizHandler : IRequestHandler<AttemptQuizCommand, APIResponse>
{
    #region DI
    private readonly IUserAnswerRepository _userAnswerRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IAnswerRepository _answerRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisCaching _caching;
    private readonly IDistributedLockProvider _synchronizationProvider;
    public AttemptQuizHandler(IUserAnswerRepository userAnswerRepository, IQuestionRepository questionRepository, 
        IAnswerRepository answerRepository, IUnitOfWork unitOfWork, IQuizRepository quizRepository, IRedisCaching caching, IDistributedLockProvider synchronizationProvider)
    {
        _userAnswerRepository = userAnswerRepository;
        _questionRepository = questionRepository;
        _answerRepository = answerRepository;
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
        _caching = caching;
        _synchronizationProvider = synchronizationProvider;
    }
    #endregion
    public async Task<APIResponse> Handle(AttemptQuizCommand request, CancellationToken cancellationToken)
    {
        /*bool isAttempted = await _userAnswerRepository.IsAttempted(request.QuizId, request.UserId);
        if (isAttempted)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageEvent.YouAlreadyAttemptedThisQuiz,
                Data = MessageEvent.YouAlreadyAttemptedThisQuiz
            };
        }
        var @lock = _synchronizationProvider.CreateLock($"Attempt_{request.UserId}_{request.QuizId}");
        await using (var handle = await @lock.TryAcquireAsync())
        {
            if (handle == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.Conflict,
                    Message = MessageCommon.lockAcquired
                };
            }
        }*/
            int attempNo = await _userAnswerRepository.GetAttemptNo(request.QuizId, request.UserId);
        int quizAttemptAllow = await _quizRepository.GetQuizAttemptAllow(request.QuizId);
        if(quizAttemptAllow > 0 && attempNo >= quizAttemptAllow)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageEvent.QuizAttemptReachedLimit,
                Data = null
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
            entity.IsCorrect = false;
            if (attempQuizDto.AnswerId != null)
            {
                entity.AnswerContent = attempQuizDto.AnswerId.ToString();
                Answer userAnswer = await _answerRepository.GetById(attempQuizDto.AnswerId);
                if (userAnswer!.IsCorrectAnswer)
                {
                    userPoint++;
                    entity.IsCorrect = true;
                }
            }
            if(attempQuizDto.AnswerContent != null) {
                entity.AnswerContent = attempQuizDto.AnswerContent;
                Question question = await _questionRepository.GetById(attempQuizDto?.QuestionId);
                /*if (question.CorrectAnswerLabel.Equals(attempQuizDto.AnswerContent, StringComparison.OrdinalIgnoreCase))
                {
                    userPoint++;
                    entity.IsCorrect = true;
                }*/
                if (!question.IsMultipleAnswers)
                {
                    entity.IsCorrect = null;
                }
            }
            entity.AttemptNo = attempNo + 1;
            entity.SubmitAt = DateTime.Now;
            await _userAnswerRepository.Add(entity);
        }
        try
        {
            if(await _unitOfWork.SaveChangesAsync() > 0)
            {
                //var result = await _questionRepository.GetQuestionsByQuizId(request.QuizId);
                //string finalResult = $"Your final score: {userPoint}/{maxPoint}";
                var invalidateCache = await _caching.SearchKeysAsync($"users_asnswers_{request.QuizId}");
                if (invalidateCache != null)
                {
                    foreach (var key in invalidateCache)
                        await _caching.DeleteKeyAsync(key);
                }
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.CreateSuccesfully,
                    Data = MessageCommon.CreateSuccesfully//_mapper.Map<List<ResponseQuizAttempt>>(result)
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
