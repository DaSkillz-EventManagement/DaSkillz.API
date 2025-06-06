﻿using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Quiz.Response;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Quizs.Commands.CreateQuiz;

public class CreateQuizHandler : IRequestHandler<CreateQuizCommand, APIResponse>
{
    private readonly IQuizRepository _quizRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateQuizHandler(IQuizRepository quizRepository, IEventRepository eventRepository,
        IUnitOfWork unitOfWork, IMapper mapper)
    {
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        #region Authen/author
        #endregion
        Quiz entity = _mapper.Map<Quiz>(request.QuizDto);
        entity.QuizId = Guid.NewGuid();
        entity.CreateAt = DateTime.Now;
        entity.CreatedBy = request.UserId;
        entity.TotalTime = request.QuizDto.TotalTime;
        entity.QuizStatus = (int)request.QuizDto.QuizStatus;
        await _quizRepository.Add(entity);

        #region Saving entity
        try
        {
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                ResponseQuizDto response = _mapper.Map<ResponseQuizDto>(entity);
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.CreateSuccesfully,
                    Data = response
                };
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageEvent.CreateQuizFailed,
                Data = request.QuizDto
            };
        }
        catch (Exception ex)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageEvent.CreateQuizFailed,
                Data = ex.Message
            };
        }
        #endregion
    }
}
