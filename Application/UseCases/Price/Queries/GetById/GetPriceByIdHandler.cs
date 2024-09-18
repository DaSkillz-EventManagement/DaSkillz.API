using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.PriceDto;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Prices.Queries.GetById;

public class GetPriceByIdHandler : IRequestHandler<GetPriceByIdQuery, APIResponse>
{
    private readonly IPriceRepository _pricerepo;
    private readonly IMapper _mapper;
    public GetPriceByIdHandler(IPriceRepository pricerepo, IMapper mapper)
    {
        _pricerepo = pricerepo;
        _mapper = mapper;
    }
    public async Task<APIResponse> Handle(GetPriceByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _pricerepo.GetById(request.PriceId);
        if (result == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
                Data = null
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = _mapper.Map<ResponsePriceDto>(result)
        };
    }
}
