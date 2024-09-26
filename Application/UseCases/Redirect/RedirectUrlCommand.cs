using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Redirect
{
    public class RedirectUrlCommand : IRequest<string>
    {
        public string? url { get; set; }
        public string? appstransid { get; set; }

        public RedirectUrlCommand(string? url, string? appstransid)
        {
            this.url = url;
            this.appstransid = appstransid;
        }
    }
}
