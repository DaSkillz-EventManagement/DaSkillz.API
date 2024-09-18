using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Prices.Commands.Delete;

public class DeletePriceCommand: IRequest<APIResponse>
{
    public int PriceId { get; set; }
    public DeletePriceCommand(int priceId)
    { 
        PriceId = priceId;
    }
}
