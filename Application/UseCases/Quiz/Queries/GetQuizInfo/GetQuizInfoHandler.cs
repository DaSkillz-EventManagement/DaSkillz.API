using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Quiz.Response;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Quizs.Queries.GetQuizInfo;

public class GetQuizInfoHandler : IRequestHandler<GetQuizInfoQuery, APIResponse>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;
    public GetQuizInfoHandler(IQuestionRepository questionRepository, IMapper mapper)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;
    }
    public async Task<APIResponse> Handle(GetQuizInfoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _questionRepository.GetQuestionsByQuizId(request.QuizId);
            if (result.Count > 0)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.Complete,
                    Data = _mapper.Map<List<ResponseQuestionDto>>(result)
                };
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = new List<ResponseQuestionDto>()
            };
        }
        catch (Exception ex)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.GetFailed,
                Data = ex.Message
            };
        }
    }
}
