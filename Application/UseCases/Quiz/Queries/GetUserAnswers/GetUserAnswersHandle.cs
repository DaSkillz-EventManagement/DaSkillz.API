using Application.Abstractions.Caching;
using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Quiz.Response;
using Domain.DTOs.User.Response;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Queries.GetUserAnswers;

public class GetUserAnswersHandle : IRequestHandler<GetUserAnswersQuery, APIResponse>
{
    private readonly IUserAnswerRepository _userAnswerRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IRedisCaching _caching;
    private readonly IMapper _mapper;
    public GetUserAnswersHandle(IUserAnswerRepository userAnswerRepository, IMapper mapper,
        IQuestionRepository questionRepository, IRedisCaching caching)
    {
        _userAnswerRepository = userAnswerRepository;
        _questionRepository = questionRepository;
        _mapper = mapper;
        _caching = caching;
    }

    public async Task<APIResponse> Handle(GetUserAnswersQuery request, CancellationToken cancellationToken)
    {

        var cacheKey = $"users_asnswers_{request.QuizId}_{request.UserId}";
        var cachingData = await _caching.GetAsync<Dictionary<string, List<UserAnswerResponseDto>>> (cacheKey);
        if (cachingData != null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = cachingData.ToList()
            };
        }

        var result = await _userAnswerRepository.GetUserAnswer(request.UserId, request.QuizId);
        Dictionary<string, List<UserAnswerResponseDto>> response = new Dictionary<string, List<UserAnswerResponseDto>>();
        if (result != null)
        {
            //List<UserAnswerResponseDto> responseDtos = new List<UserAnswerResponseDto>();
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
                if (!response.ContainsKey($"attempNo{item.AttemptNo}"))
                {
                    response[$"attempNo{item.AttemptNo}"] = new List<UserAnswerResponseDto>();
                }
                response[$"attempNo{item.AttemptNo}"].Add(responseDto);
            }
            await _caching.SetAsync(cacheKey, response, 2);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = response.ToList(),
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
