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
                Question question = await _questionRepository.GetById(item.QuestionId); 
                responseDto.Question = _mapper.Map<UserAnswerResultDto>(question);
                responseDto = _mapper.Map<UserAnswerResponseDto>(item);
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
