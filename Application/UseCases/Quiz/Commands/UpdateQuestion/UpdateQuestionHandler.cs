﻿using Application.ResponseMessage;
using AutoMapper;
using Azure;
using Domain.DTOs.Quiz.Request;
using Domain.DTOs.Quiz.Response;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Quizs.Commands.UpdateQuestion;

public class UpdateQuestionHandler : IRequestHandler<UpdateQuestionCommand, APIResponse>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;
    private readonly IAnswerRepository _answerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateQuestionHandler(IQuestionRepository questionRepository, IMapper mapper, IAnswerRepository answerRepository, 
        IUnitOfWork unitOfWork)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;
        _answerRepository = answerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        foreach(var item in request.Question)
        {   
                Question? temp = await _questionRepository.GetById(item.QuestionId);
                if (temp != null)
                {
                    temp.QuestionName = item.QuestionName;
                    temp.CorrectAnswerLabel = string.Empty;
                    temp.IsMultipleAnswers = item.IsMultipleAnswers;
                    await _questionRepository.Update(temp);
                    foreach (var answer in item.Answers)
                    {
                        var entity = await _answerRepository.GetByIdAsNoTracking(answer.AnswerId);
                        if (entity != null)
                        {
                            entity.AnswerContent = answer.AnswerContent;
                            entity.IsCorrectAnswer = answer.IsCorrectAnswer;
                            await _answerRepository.Update(entity);
                    }
                    else
                    {
                        Answer newAnswer = new Answer
                        {
                            AnswerId = answer.AnswerId,
                            QuestionId = item.QuestionId,
                            AnswerContent = answer.AnswerContent,
                            IsCorrectAnswer = answer.IsCorrectAnswer
                        };
                        await _answerRepository.Add(newAnswer);
                    }
                    }
                }
            else
            {
                Question newQuestion = new Question
                {
                    QuestionId = item.QuestionId,
                    QuestionName = item.QuestionName,
                    CorrectAnswerLabel = string.Empty,
                    IsMultipleAnswers = item.IsMultipleAnswers,
                    ShowAnswerAfterChoosing = false,
                    QuizId = request.QuizId
                };
                await _questionRepository.Add(newQuestion);
                foreach (var answer in item.Answers)
                {
                    Answer newAnswer = new Answer
                    {
                        AnswerId = answer.AnswerId,
                        QuestionId = newQuestion.QuestionId,
                        AnswerContent = answer.AnswerContent,
                        IsCorrectAnswer = answer.IsCorrectAnswer
                    };
                    await _answerRepository.Add(newAnswer);
                }
            }

        }
        try
        {
            if((await _unitOfWork.SaveChangesAsync() > 0))
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message =MessageCommon.UpdateSuccesfully,
                    Data = null
                };
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.UpdateFailed,
                Data = null
            };
        }
        catch (Exception ex)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.UpdateFailed,
                Data = ex.Message
            };
        }
    }
}
