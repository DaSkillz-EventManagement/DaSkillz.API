using Domain.DTOs.PriceDto;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Prices.Commands.Create;

public class CreatePriceCommand : IRequest<APIResponse>
{
    public Guid UserId { get; set; }
    public PriceDto Dto { get; set; }
    public CreatePriceCommand(Guid userId, PriceDto dto)
    {
        UserId = userId;
        Dto = dto;
    }
}
