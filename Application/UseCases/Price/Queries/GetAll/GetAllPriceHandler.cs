using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.PriceDto;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Prices.Queries.GetAll;

public class GetAllPriceHandler : IRequestHandler<GetAllPriceQuery, APIResponse>
{
    private readonly IPriceRepository _pricerepo;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;
    public GetAllPriceHandler(IPriceRepository pricerepo, IMapper mapper, IUserRepository userRepo)
    {
        _pricerepo = pricerepo;
        _mapper = mapper;
        _userRepo = userRepo;
    }
    public async Task<APIResponse> Handle(GetAllPriceQuery request, CancellationToken cancellationToken)
    {
        var result = await _pricerepo.GetAllPrice(request.OrderBy, request.IsAscending);
        if (result == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = null
            };
        }
        List<ResponsePriceDto> responses = new List<ResponsePriceDto>();
        foreach (var item in result)
        {
            ResponsePriceDto response = new ResponsePriceDto();
            response.PriceId = item.PriceId;
            response.PriceType = item.PriceType;
            response.note = item.note;
            response.amount = item.amount;
            response.UpdatedAt = item.UpdatedAt;
            response.CreatedAt = item.CreatedAt;
            var user = await _userRepo.GetById(item.CreatedBy);
            response.CreatedBy.email = user!.Email!;
            response.CreatedBy.Name = user.FullName;
            response.CreatedBy.avatar = user.Avatar;
            response.CreatedBy.Id = user.UserId;
            responses.Add(response);
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = responses
        };
    }
}
