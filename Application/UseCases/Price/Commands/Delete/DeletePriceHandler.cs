using AutoMapper;
using Domain.Models.Response;
using Domain.Repositories.UnitOfWork;
using Domain.Repositories;
using MediatR;
using Domain.Entities;
using Application.ResponseMessage;
using Domain.DTOs.PriceDto;
using Domain.Enum.Price;
using System.Net;

namespace Application.UseCases.Prices.Commands.Delete;

public class DeletePriceHandler : IRequestHandler<DeletePriceCommand, APIResponse>
{
    private readonly IPriceRepository _pricerepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public DeletePriceHandler(IPriceRepository pricerepo, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _pricerepo = pricerepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<APIResponse> Handle(DeletePriceCommand request, CancellationToken cancellationToken)
    {
        Price? price = await _pricerepo.GetById(request.PriceId);
        if(price != null)
        {
            price.status = PriceStatus.deteleted.ToString();
            await _pricerepo.Update(price);
            if(await _unitOfWork.SaveChangesAsync() > 0) 
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.DeleteSuccessfully,
                    Data = _mapper.Map<ResponsePriceDto>(price)
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
