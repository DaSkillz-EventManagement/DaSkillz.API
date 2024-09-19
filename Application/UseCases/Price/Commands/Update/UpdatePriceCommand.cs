using Domain.DTOs.PriceDto;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Prices.Commands.Update;

public class UpdatePriceCommand: IRequest<APIResponse>
{
    public UpdatePriceDto? PriceDto { get; set; }
    public UpdatePriceCommand(UpdatePriceDto? priceDto)
    {
        PriceDto = priceDto;
    }
}
