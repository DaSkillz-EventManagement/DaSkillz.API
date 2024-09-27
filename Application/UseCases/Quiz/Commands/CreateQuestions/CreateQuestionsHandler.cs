using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Quiz.Response;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

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
        /*var isOwner = await _eventRepository.IsOwner(request.EventId, request.UserId);
        if (!isOwner)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageEvent.OnlyHostCanCreateQuiz,
                Data = null
            };
        }*/
        #endregion

        if (request.Dtos == null || !request.Dtos.Any())
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageEvent.OnlyHostCanCreateQuiz,
                Data = null
            };
        }

        
        var tasks = new List<Task>();

        foreach (var question in request.Dtos)
        {
            Question temp = new Question
            {
                QuestionId = Guid.NewGuid(),
                QuestionName = question.QuestionName,
                QuizId = request.QuizId,
                CorrectAnswerLabel = question.CorrectAnswerLabel,
                IsMultipleAnswers = question.IsMultipleAnswers,
                ShowAnswerAfterChoosing = question.ShowAnswerAfterChoosing
            };
            tasks.Add(_questionRepository.Add(temp));

            foreach (var answer in question.Answers)
            {
                Answer entity = new Answer
                {
                    AnswerId = Guid.NewGuid(),
                    QuestionId = temp.QuestionId,
                    AnswerContent = answer.Content,
                    IsCorrectAnswer = answer.IsCorrectAnswer
                };
                tasks.Add(_answerRepository.Add(entity));
                temp.Answers.Add(entity);
            }
        }

        #region Saving entity
        try
        {
            await Task.WhenAll(tasks); // wait all tasks to complete           
            return new APIResponse
            {
                StatusResponse = (await _unitOfWork.SaveChangesAsync() > 0) ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                Message = (await _unitOfWork.SaveChangesAsync() > 0) ? MessageCommon.CreateSuccesfully : MessageCommon.CreateFailed,
                Data = null
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
