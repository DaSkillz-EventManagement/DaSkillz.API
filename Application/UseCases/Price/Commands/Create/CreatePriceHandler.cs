using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.PriceDto;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Prices.Commands.Create;

public class CreatePriceHandler : IRequestHandler<CreatePriceCommand, APIResponse>
{
    private readonly IPriceRepository _pricerepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreatePriceHandler(IPriceRepository pricerepo, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _pricerepo = pricerepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(CreatePriceCommand request, CancellationToken cancellationToken)
    {
        Price price = _mapper.Map<Price>(request.Dto);
        price.CreatedAt = DateTime.Now;
        price.CreatedBy = request.UserId;
        _ = _pricerepo.Add(price);
        if(await _unitOfWork.SaveChangesAsync() > 0)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.Created,
                Message = MessageCommon.Complete,
                Data = _mapper.Map<ResponsePriceDto>(price)
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.BadRequest,
            Message = MessageCommon.CreateFailed,
            Data = null
        };
    }
}
