using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.PriceDto;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Prices.Queries.GetById;

public class GetPriceByIdHandler : IRequestHandler<GetPriceByIdQuery, APIResponse>
{
    private readonly IPriceRepository _pricerepo;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;
    public GetPriceByIdHandler(IPriceRepository pricerepo, IMapper mapper, IUserRepository userRepo)
    {
        _pricerepo = pricerepo;
        _mapper = mapper;
        _userRepo = userRepo;
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
        ResponsePriceDto response = new ResponsePriceDto();
        response.PriceId = result.PriceId;
        response.PriceType = result.PriceType;
        response.note = result.note;
        response.amount = result.amount;
        response.unit = result.unit;
        response.UpdatedAt = result.UpdatedAt;
        response.CreatedAt = result.CreatedAt;
        var user = await _userRepo.GetById(result.CreatedBy);
        response.CreatedBy!.email = user!.Email!;
        response.CreatedBy.Name = user.FullName;
        response.CreatedBy.avatar = user.Avatar;
        response.CreatedBy.Id = user.UserId;
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = response
        };
    }
}
