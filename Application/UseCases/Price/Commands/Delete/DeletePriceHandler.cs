﻿using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.PriceDto;
using Domain.Entities;
using Domain.Enum.Price;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Prices.Commands.Delete;

public class DeletePriceHandler : IRequestHandler<DeletePriceCommand, APIResponse>
{
    private readonly IPriceRepository _pricerepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;
    public DeletePriceHandler(IPriceRepository pricerepo, IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepo)
    {
        _pricerepo = pricerepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userRepo = userRepo;
    }
    public async Task<APIResponse> Handle(DeletePriceCommand request, CancellationToken cancellationToken)
    {
        Price? price = await _pricerepo.GetById(request.PriceId);
        if (price != null)
        {
            price.status = PriceStatus.deteleted.ToString();
            await _pricerepo.Update(price);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                ResponsePriceDto response = new ResponsePriceDto();
                response.PriceId = price.PriceId;
                response.PriceType = price.PriceType;
                response.note = price.note;
                response.amount = price.amount;
                response.unit = price.unit;
                response.UpdatedAt = price.UpdatedAt;
                response.CreatedAt = price.CreatedAt;
                var user = await _userRepo.GetById(price.CreatedBy);
                response.CreatedBy.email = user!.Email!;
                response.CreatedBy.Name = user.FullName;
                response.CreatedBy.avatar = user.Avatar;
                response.CreatedBy.Id = user.UserId;
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.DeleteSuccessfully,
                    Data = response
                };
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.DeleteFailed,
                Data = null
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
