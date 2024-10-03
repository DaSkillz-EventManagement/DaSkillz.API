using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Quiz.Response;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Queries.ShowQuestion;

public class ShowQuestionHanlder : IRequestHandler<ShowQuestionQuery, APIResponse>
{ 
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public ShowQuestionHanlder(IQuestionRepository questionRepository, IMapper mapper)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(ShowQuestionQuery request, CancellationToken cancellationToken)
    {
        var result = await _questionRepository.GetQuestionsByQuizId(request.QuizId);
        if(result != null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.CreateSuccesfully,
                Data = _mapper.Map<List<ResponseQuizAttempt>>(result)
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.CreateSuccesfully,
            Data = new List<ResponseQuizAttempt>()
        };
    }
}
