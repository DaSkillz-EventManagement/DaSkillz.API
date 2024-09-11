using Application.ExternalServices.Images;
using Application.Helper;
using Application.ResponseMessage;
using AutoMapper;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Events.Command.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, APIResponse>
    {

        private readonly IEventRepository _eventRepo;
        private readonly IImageService _fileService;
        private readonly IMapper _mapper;
        private readonly long dateTimeConvertValue = 25200000; //-7h to match JS dateTime type
        private readonly long minimumUpdateTimeSpan = 21600000;//time span between event created and new event startDate

        public CreateEventCommandHandler(IEventRepository eventRepo, IImageService fileService, IMapper mapper)
        {

            _eventRepo = eventRepo;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var tempStartDate = DateTimeOffset.FromUnixTimeMilliseconds(request.EventRequestDto.StartDate).DateTime;
            if (tempStartDate > DateTime.Now.AddMonths(4))
            {
                return new APIResponse
                {
                    Message = MessageEvent.StarTimeValidation,
                    StatusResponse = HttpStatusCode.BadRequest
                };
            }
            bool validate = DateTimeHelper.ValidateStartTimeAndEndTime(request.EventRequestDto.StartDate, request.EventRequestDto.EndDate);
            if (!validate)
            {
                return new APIResponse
                {
                    Message = MessageEvent.StartEndTimeValidation,
                    StatusResponse = HttpStatusCode.BadRequest
                };
            }
            var eventEntity = _mapper.Map<Event>(request);
            eventEntity.Id = Guid.NewGuid();
            eventEntity.StartDate = request.EventRequestDto.StartDate + dateTimeConvertValue;
            eventEntity.EndDate = request.EventRequestDto.EndDate + dateTimeConvertValue;
            if (request.EventRequestDto.Image != null)
            {
                eventEntity.Image = await _fileService.UploadImage(request.EventRequestDto.Image, Guid.NewGuid());
            }




            return new APIResponse
            {
                Message = MessageCommon.CreateFailed,
                StatusResponse = HttpStatusCode.BadRequest
            };
        }
    }
}
