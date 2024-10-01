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
    private readonly IUserRepository _userRepo;
    public CreatePriceHandler(IPriceRepository pricerepo, IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepo)
    {
        _pricerepo = pricerepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userRepo = userRepo;
    }

    public async Task<APIResponse> Handle(CreatePriceCommand request, CancellationToken cancellationToken)
    {
        Price price = _mapper.Map<Price>(request.Dto);
        price.CreatedAt = DateTime.Now;
        price.CreatedBy = request.UserId;
        _ = _pricerepo.Add(price);
        if (await _unitOfWork.SaveChangesAsync() > 0)
        {
            ResponsePriceDto response = new ResponsePriceDto();
            response.PriceId = price.PriceId;
            response.PriceType = price.PriceType;
            response.note = price.note;
            response.unit = price.unit;
            response.amount = price.amount;
            response.UpdatedAt = price.UpdatedAt;
            response.CreatedAt = price.CreatedAt;
            var user = await _userRepo.GetById(price.CreatedBy);
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
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.BadRequest,
            Message = MessageCommon.CreateFailed,
            Data = null
        };
    }
}
