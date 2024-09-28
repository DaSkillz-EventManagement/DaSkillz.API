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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        quiz.QuizName = request.QuizDto.QuizName;
        quiz.QuizDescription = request.QuizDto.QuizDescription;
        quiz.status = request.QuizDto.status;
        quiz.TotalTime = request.QuizDto.TotalTime;
        quiz.AttemptAllow = request.QuizDto.AttemptAllow;
        try
        {
            APIResponse response = new APIResponse();
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.UpdateSuccesfully;
                response.Data = _mapper.Map<ResponseQuizDto>(quiz);
                return response;
            }
            response.StatusResponse = HttpStatusCode.BadRequest;
            response.Message = MessageCommon.UpdateFailed;
            response.Data = _mapper.Map<ResponseQuizDto>(quiz);
            return response;
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
