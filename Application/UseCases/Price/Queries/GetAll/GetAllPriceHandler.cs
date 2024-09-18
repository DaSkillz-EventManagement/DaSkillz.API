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
using System.Net;
using Application.ResponseMessage;
using Domain.DTOs.PriceDto;

namespace Application.UseCases.Prices.Queries.GetAll;

public class GetAllPriceHandler : IRequestHandler<GetAllPriceQuery, APIResponse>
{
    private readonly IPriceRepository _pricerepo;
    private readonly IMapper _mapper;
    public GetAllPriceHandler(IPriceRepository pricerepo, IMapper mapper)
    {
        _pricerepo = pricerepo;
        _mapper = mapper;
    }
    public async Task<APIResponse> Handle(GetAllPriceQuery request, CancellationToken cancellationToken)
    {
        var result = await _pricerepo.GetAllPrice(request.OrderBy, request.IsAscending);
        if(result == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = null
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = _mapper.Map<List<ResponsePriceDto>>(result)
        };
    }
}
