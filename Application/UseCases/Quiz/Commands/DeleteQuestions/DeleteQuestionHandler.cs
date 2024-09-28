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

namespace Application.UseCases.Quizs.Commands.DeleteQuestions;

public class DeleteQuestionHandler : IRequestHandler<DeleteQuestionCommand, APIResponse>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IUserAnswerRepository _userAnswerRepository;
    private readonly IMapper _mapper;

    public DeleteQuestionHandler(IQuestionRepository questionRepository, IMapper mapper, IUserAnswerRepository userAnswerRepository)
    {
        _questionRepository = questionRepository;
        _userAnswerRepository = userAnswerRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        List<ResponseQuestionDto> responseQuestions = new List<ResponseQuestionDto>();
        var questions = await _questionRepository.DeleteQuestions(request.QuestionID);
        if(questions.Count > 0)
        {
            responseQuestions = _mapper.Map<List<ResponseQuestionDto>>(questions);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.DeleteSuccessfully,
                Data = responseQuestions
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.BadRequest,
            Message = MessageCommon.DeleteFailed,
            Data = responseQuestions
        };
    }
}
