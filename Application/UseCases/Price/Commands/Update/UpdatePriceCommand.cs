using Domain.DTOs.PriceDto;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Prices.Commands.Update;

public class UpdatePriceCommand : IRequest<APIResponse>
{
    public UpdatePriceDto? PriceDto { get; set; }
    public UpdatePriceCommand(UpdatePriceDto? priceDto)
    {
        PriceDto = priceDto;
    }
}
