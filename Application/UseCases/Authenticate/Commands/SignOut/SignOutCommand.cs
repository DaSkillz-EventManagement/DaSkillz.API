using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Authenticate.Commands.SignOut
{
    public class SignOutCommand : IRequest<APIResponse>
    {
        public string userId { get; set; }

        public SignOutCommand(string userId)
        {
            this.userId = userId;
        }
    }
}
