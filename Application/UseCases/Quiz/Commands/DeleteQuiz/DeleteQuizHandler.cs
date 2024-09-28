using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Quiz.Response;
using Domain.Enum.Quiz;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Commands.DeleteQuiz;

public class DeleteQuizHandler : IRequestHandler<DeleteQuizCommand, APIResponse>
{
    private readonly IEventRepository _eventRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteQuizHandler(IEventRepository eventRepository, IQuizRepository quizRepository, IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(DeleteQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetById(request.QuizId);
        if(quiz == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.DeleteFailed,
                Data = request.QuizId.ToString()
            };
        }
        quiz.status = (int)QuizEnum.Deleted;
        await _quizRepository.Update(quiz);
        try
        {
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.DeleteSuccessfully,
                    Data = _mapper.Map<ResponseQuizDto>(quiz)
                };
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.DeleteFailed,
                Data = request.QuizId.ToString()
            };
        }catch (Exception ex)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.DeleteFailed,
                Data = ex.Message
            };
        }
    }
}
