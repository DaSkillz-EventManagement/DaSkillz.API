using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Authenticate.Commands.SignInWithGoogle
{
    public class SignInGoogleCommand : IRequest<APIResponse>
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Token { get; set; }
        public string PhotoUrl { get; set; } = null!;

        public SignInGoogleCommand(string fullName, string email, string? token, string photoUrl)
        {
            FullName = fullName;
            Email = email;
            Token = token;
            PhotoUrl = photoUrl;
        }
    }
}
