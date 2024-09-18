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

namespace Application.UseCases.Prices.Commands.Update;

public class UpdatePriceHandler : IRequestHandler<UpdatePriceCommand, APIResponse>
{
    private readonly IPriceRepository _pricerepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdatePriceHandler(IPriceRepository pricerepo, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _pricerepo = pricerepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
        if(await _unitOfWork.SaveChangesAsync() > 0)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.UpdateSuccesfully,
                Data = _mapper.Map<ResponsePriceDto>(entity)
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
