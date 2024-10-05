using Application.Abstractions.Caching;
using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Quiz.Response;
using Domain.DTOs.User.Response;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Queries.GetAllUsersAnswers;

public class GetAllUsersAnswersHandler : IRequestHandler<GetAllUsersAnswersQuery, APIResponse>
{
    private readonly IUserAnswerRepository _userAnswerRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IRedisCaching _caching;
    private readonly IMapper _mapper;
    public GetAllUsersAnswersHandler(IUserAnswerRepository userAnswerRepository, IMapper mapper,
        IQuestionRepository questionRepository, IRedisCaching caching)
    {
        _userAnswerRepository = userAnswerRepository;
        _questionRepository = questionRepository;
        _mapper = mapper;
        _caching = caching;
    }
    public async Task<APIResponse> Handle(GetAllUsersAnswersQuery request, CancellationToken cancellationToken)
    {

        var cacheKey = $"all_users_asnswers_{request.QuizId}";
        var cachingData = await _caching.GetAsync<List<AllUsersAnswersDto>>(cacheKey);
        if (cachingData != null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = cachingData.ToList()
            };
        }



        List<AllUsersAnswersDto> response = new List<AllUsersAnswersDto>();
        var userParticipated = await _userAnswerRepository.GetListUsersAttemptedQuiz(request.QuizId);
        foreach(var user in userParticipated)
        {
            AllUsersAnswersDto temp = new AllUsersAnswersDto();
            temp.userInfo = _mapper.Map<AttemptedQuizUserResponse>(user);
            Dictionary<string, List<UserAnswerResponseDto>> dictionary = new Dictionary<string, List<UserAnswerResponseDto>>();
            var result = await _userAnswerRepository.GetUserAnswer(user.UserId, request.QuizId);
            //List<UserAnswerResponseDto> responseDtos = new List<UserAnswerResponseDto>();
            if (result != null)
            {   
                foreach (var item in result)
                {
                    UserAnswerResponseDto responseDto = new UserAnswerResponseDto();
                    Question question = await _questionRepository.GetQuestionById(item.QuestionId);
                    responseDto.Question = _mapper.Map<ResponseQuestionDto>(question);
                    responseDto = _mapper.Map<UserAnswerResponseDto>(item);
                    if (question.IsMultipleAnswers)
                    {
                        responseDto.AnswerId = item.AnswerContent != null ? Guid.Parse(item.AnswerContent) : null;
                        responseDto.AnswerContent = null;
                    }
                    if (!question.IsMultipleAnswers)
                    {
                        responseDto.AnswerId = null;
                        responseDto.AnswerContent = item.AnswerContent;
                    }
                    //responseDtos.Add(responseDto);
                    if (!dictionary.ContainsKey($"attempNo{item.AttemptNo}"))
                    {
                        dictionary[$"attempNo{item.AttemptNo}"] = new List<UserAnswerResponseDto>();
                    }
                    dictionary[$"attempNo{item.AttemptNo}"].Add(responseDto);
                }
                temp.userAnswers = dictionary.ToList();
            }
            response.Add(temp);
        }
        await _caching.SetAsync(cacheKey, response, 2);

        if (response != null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = response
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = new List<AttemptedQuizUserResponse>()
        };
    }
}
