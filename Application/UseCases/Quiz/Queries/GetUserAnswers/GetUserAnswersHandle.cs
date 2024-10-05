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
    private readonly IMapper _mapper;
    public GetUserAnswersHandle(IUserAnswerRepository userAnswerRepository, IMapper mapper,
        IQuestionRepository questionRepository)
    {
        _userAnswerRepository = userAnswerRepository;
        _questionRepository = questionRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetUserAnswersQuery request, CancellationToken cancellationToken)
    {
        var result = await _userAnswerRepository.GetUserAnswer(request.UserId, request.QuizId);
        if (result != null)
        {
            List<UserAnswerResponseDto> responseDtos = new List<UserAnswerResponseDto>();
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
                responseDtos.Add(responseDto);
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = responseDtos
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
