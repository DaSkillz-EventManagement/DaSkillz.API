using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Quiz.Response;
using Domain.Entities;
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

namespace Application.UseCases.Quizs.Commands.UpdateQuiz;

public class UpdateQuizHandler : IRequestHandler<UpdateQuizCommand, APIResponse>
{
    private IQuizRepository _quizRepository;
    private IMapper _mapper;
    private IUnitOfWork _unitOfWork;

    public UpdateQuizHandler(IQuizRepository quizRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _quizRepository = quizRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetById(request.QuizId);
        if(quiz == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.NotFound,
                Data = null
            };
        }
        quiz = _mapper.Map<Quiz>(request.QuizDto);
        try
        {
            return new APIResponse
            {
                StatusResponse = (await _unitOfWork.SaveChangesAsync() > 0) ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                Message = (await _unitOfWork.SaveChangesAsync() > 0) ? MessageCommon.UpdateSuccesfully : MessageCommon.UpdateFailed,
                Data = _mapper.Map<ResponseQuizDto>(quiz)
            };
        } catch (Exception ex)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.UpdateFailed,
                Data = ex.Message
            };
        }
    }
}
