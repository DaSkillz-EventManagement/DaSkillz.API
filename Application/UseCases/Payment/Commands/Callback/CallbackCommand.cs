using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Payment.Commands.Callback
{
    public class CallbackCommand : IRequest<object>
    {
        public dynamic? requestBody { get; set; }
    }
}
