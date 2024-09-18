using AutoMapper;
using Domain.Models.Response;
using Domain.Repositories.UnitOfWork;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.ResponseMessage;
using System.Net;
using Domain.DTOs.PriceDto;
using Elastic.Clients.Elasticsearch.Security;

namespace Application.UseCases.Prices.Commands.Update;

public class UpdatePriceHandler : IRequestHandler<UpdatePriceCommand, APIResponse>
{
    private readonly IPriceRepository _pricerepo;
    private readonly IUserRepository _userRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdatePriceHandler(IPriceRepository pricerepo, IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepo)
    {
        _pricerepo = pricerepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userRepo = userRepo;
    }
    public async Task<APIResponse> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
    {
        Price entity = await _pricerepo.GetById(request.PriceDto.PriceId);
        if (entity == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
                Data = null
            };
        }
        entity.amount = request.PriceDto.amount;
        entity.note = request.PriceDto.note!;
        entity.PriceType = request.PriceDto.PriceType.ToString();
        entity.UpdatedAt = DateTime.Now;
        ResponsePriceDto response = new ResponsePriceDto();
        response.PriceId = entity.PriceId;
        response.PriceType = entity.PriceType;
        response.note = entity.note;
        response.amount = entity.amount;
        response.UpdatedAt = entity.UpdatedAt;
        response.CreatedAt = entity.CreatedAt;
        var user = await _userRepo.GetById(entity.CreatedBy);
        response.CreatedBy.email = user!.Email!;
        response.CreatedBy.Name = user.FullName;
        response.CreatedBy.avatar = user.Avatar; 
        response.CreatedBy.Id = user.UserId;
        if (await _unitOfWork.SaveChangesAsync() > 0)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.UpdateSuccesfully,
                Data = response
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.BadRequest,
            Message = MessageCommon.UpdateFailed,
            Data = null,
        };
    }
}
