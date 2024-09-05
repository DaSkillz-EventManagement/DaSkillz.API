using Application.Message;
using AutoMapper;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repository.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase.Events.Command
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventRepo _eventRepo;
        private readonly IMapper _mapper;

        public CreateEventCommandHandler(IUnitOfWork unitOfWork, IEventRepo eventRepo, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _eventRepo = eventRepo;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var tempStartDate = DateTimeOffset.FromUnixTimeMilliseconds(request.StartDate).DateTime;
            if (tempStartDate > DateTime.Now.AddMonths(4))
            {
                return new APIResponse
                {
                    Message = MessageEvent.StarTimeValidation,
                    StatusResponse = HttpStatusCode.BadRequest
                };
            }
            return new APIResponse
            {
                Message = MessageCommon.CreateFailed,
                StatusResponse = HttpStatusCode.BadRequest
            };
        }
    }
}
