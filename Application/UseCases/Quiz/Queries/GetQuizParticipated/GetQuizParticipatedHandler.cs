using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.User.Response;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Queries.GetQuizParticipated;

public class GetQuizParticipatedHandler : IRequestHandler<GetQuizParticipatedQuery, APIResponse>
{
    private readonly IUserAnswerRepository _answerRepository;
    private readonly IMapper _mapper;

    public GetQuizParticipatedHandler(IUserAnswerRepository answerRepository, IMapper mapper)
    {
        _answerRepository = answerRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetQuizParticipatedQuery request, CancellationToken cancellationToken)
    {
        var result = await _answerRepository.GetListUsersAttemptedQuiz(request.QuizId);
        if(result != null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = _mapper.Map<List<AttemptedQuizUserResponse>>(result)
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
