using Application.ResponseMessage;
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
                Question temp = await _questionRepository.GetQuestionById(item.QuestionId);
                if (temp != null)
                {
                    temp.QuestionName = item.QuestionName;
                    temp.CorrectAnswerLabel = string.Empty;
                    temp.IsMultipleAnswers = item.IsMultipleAnswers;
                    await _questionRepository.Update(temp);
                    foreach (var answer in item.Answers)
                    {
                        var entity = await _answerRepository.GetById(answer.AnswerId);
                        if (entity != null)
                        {
                            entity.AnswerContent = answer.AnswerContent;
                            entity.IsCorrectAnswer = answer.IsCorrectAnswer;
                            await _answerRepository.Update(entity);
                        }
                    }
                }
            else
            {
                Question newQuestion = new Question();
                newQuestion.QuestionId = item.QuestionId;
                newQuestion.QuestionName = item.QuestionName;
                newQuestion.CorrectAnswerLabel = string.Empty;
                newQuestion.IsMultipleAnswers= item.IsMultipleAnswers;
                newQuestion.ShowAnswerAfterChoosing = false;
                newQuestion.QuizId = request.QuizId;
                await _questionRepository.Add(newQuestion);
                foreach (var answer in item.Answers)
                {                  
                    Answer entity = new Answer();
                    entity.AnswerId = answer.AnswerId;
                    entity.QuestionId = newQuestion.QuestionId;
                    entity.AnswerContent = answer.AnswerContent;
                    entity.IsCorrectAnswer = answer.IsCorrectAnswer;
                    await _answerRepository.Add(entity);
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
