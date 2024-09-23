using AutoMapper;
using Domain.Models.Response;
using Domain.Repositories.UnitOfWork;
using Domain.Repositories;
using MediatR;
using Application.ResponseMessage;
using Domain.DTOs.Quiz.Response;
using System.Net;


namespace Application.UseCases.Quizs.Queries.GetQuizByEventId;

public class GetQuizByEventIdHandler : IRequestHandler<GetQuizByEventIdQuery, APIResponse>
{
    private readonly IQuizRepository _quizRepository;
    private readonly IMapper _mapper;
    public GetQuizByEventIdHandler(IQuizRepository quizRepository, IMapper mapper)
    {
        _quizRepository = quizRepository;
        _mapper = mapper;
    }
    public async Task<APIResponse> Handle(GetQuizByEventIdQuery request, CancellationToken cancellationToken)
    {
        var quizs = await _quizRepository.GetAllQuizsByEventId(request.EventId);
        if(quizs.Count > 0)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = _mapper.Map<List<ResponseQuizDto>>(quizs)
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.NotFound,
            Message = MessageCommon.NotFound,
            Data = null
        };
    }
}
