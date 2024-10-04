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
    private readonly IQuizRepository _quizRepository;
    private readonly IMapper _mapper;

    public ShowQuestionHanlder(IQuestionRepository questionRepository, IMapper mapper, IQuizRepository quizRepository)
    {
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(ShowQuestionQuery request, CancellationToken cancellationToken)
    {
        ShowQuestionDto result = new ShowQuestionDto();
        var questions = await _questionRepository.GetQuestionsByQuizId(request.QuizId);
        var quiz = await _quizRepository.GetById(request.QuizId);

        if(quiz != null)
        {
            
            result.Quiz = _mapper.Map<ResponseQuizDto>(quiz);
            result.Questions = _mapper.Map<List<ResponseQuizAttempt>>(questions);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = result
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = null
        };
    }
}
