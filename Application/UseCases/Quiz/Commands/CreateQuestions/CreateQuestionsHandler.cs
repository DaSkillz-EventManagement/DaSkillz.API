using AutoMapper;
using Domain.Models.Response;
using Domain.Repositories.UnitOfWork;
using Domain.Repositories;
using MediatR;
using Application.ResponseMessage;
using System.Net;
using Domain.Entities;
using Domain.DTOs.Quiz.Response;

namespace Application.UseCases.Quizs.Commands.CreateQuestions;

public class CreateQuestionsHandler : IRequestHandler<CreateQuestionsCommand, APIResponse>
{
    private readonly IQuizRepository _quizRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IAnswerRepository _answerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateQuestionsHandler(IQuizRepository quizRepository, IEventRepository eventRepository,
        IUnitOfWork unitOfWork, IMapper mapper, IQuestionRepository questionRepository, IAnswerRepository answerRepository)
    {
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
        _eventRepository = eventRepository;
        _mapper = mapper;
        _questionRepository = questionRepository;
        _answerRepository = answerRepository;
    }
    public async Task<APIResponse> Handle(CreateQuestionsCommand request, CancellationToken cancellationToken)
    {
        #region Authen/author
        Quiz? quiz = await _quizRepository.GetById(request.QuizId);
        if (quiz == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageEvent.QuizNotFound,
                Data = null
            };
        }
        var isOwner = await _eventRepository.IsOwner(request.UserId, quiz!.QuizId);
        if (!isOwner)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageEvent.OnlyHostCanCreateQuiz,
                Data = null
            };
        }
        #endregion

        List<ResponseQuestionDto> response = new List<ResponseQuestionDto>();
        foreach(var question in request.Dtos)
        {
            Question temp = new Question();
            temp.QuestionId = Guid.NewGuid();
            temp.QuestionName = question.QuestionName;
            temp.CorrectAnswerLabel = question.CorrectAnswerLabel;
            temp.IsMultipleAnswers = question.IsMultipleAnswers;
            temp.IsQuestionAnswered = question.IsQuestionAnswered;
            temp.ShowAnswerAfterChoosing = question.ShowAnswerAfterChoosing;
            await _questionRepository.Add(temp);
            foreach(var answer in question.Answers)
            {
                Answer entity = new Answer();
                entity.AnswerId = Guid.NewGuid();
                entity.QuestionId = temp.QuestionId;
                entity.AnswerLabel = answer.AnswerLabel;
                entity.Content = answer.Content;
                entity.IsCorrectAnswer = answer.IsCorrectAnswer;
                await _answerRepository.Add(entity);
            }
            ResponseQuestionDto dto = _mapper.Map<ResponseQuestionDto>(temp);
            response.Add(dto);
        }
        #region Saving entity
        try
        {
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.CreateSuccesfully,
                    Data = response
                };
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.CreateFailed,
                Data = request.Dtos
            };
        }
        catch (Exception ex)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.CreateFailed,
                Data = ex.Message
            };
        }
        #endregion
    }
}
