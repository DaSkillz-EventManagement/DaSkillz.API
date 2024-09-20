using Application.ExternalServices.Images;
using Application.ExternalServices.Quartz;
using Application.Helper;
using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Events.ResponseDto;
using Domain.Entities;
using Domain.Enum.Events;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Elastic.Clients.Elasticsearch.Security;
using MediatR;
using System.Net;
using System.Text.RegularExpressions;

namespace Application.UseCases.Events.Command.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, APIResponse>
    {

        private readonly IEventRepository _eventRepo;
        private readonly IImageService _fileService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITagRepository _tagRepository;
        private readonly IQuartzService _quartzService;

        public CreateEventCommandHandler(IEventRepository eventRepo, IImageService fileService, IMapper mapper, IUnitOfWork unitOfWork, ITagRepository tagRepository, IQuartzService quartzService)
        {
            _eventRepo = eventRepo;
            _fileService = fileService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _tagRepository = tagRepository;
            _quartzService = quartzService;
        }

        private readonly long minimumUpdateTimeSpan = 21600000;//time span between event created and new event startDate

       

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
            var eventEntity = _mapper.Map<Event>(request.EventRequestDto);
            eventEntity.Id = Guid.NewGuid();
            eventEntity.StartDate = request.EventRequestDto.StartDate;
            eventEntity.EndDate = request.EventRequestDto.EndDate;
            eventEntity.CreatedAt = DateTimeHelper.GetCurrentTimeAsLong();
            eventEntity.UpdatedAt = DateTimeHelper.GetCurrentTimeAsLong();
            eventEntity.CreatedBy = request.UserId;
            if (request.EventRequestDto.Image != null)
            {
                eventEntity.Image = await _fileService.UploadImage(request.EventRequestDto.Image, Guid.NewGuid());
            }
           

            if (!IsValidCoordinateString(request.EventRequestDto.Location.Coord!))
            {
                return new APIResponse
                {
                    Message = MessageEvent.LocationCoordInvalid,
                    StatusResponse = HttpStatusCode.BadRequest
                };
            }
            eventEntity.LocationCoord = request.EventRequestDto.Location.Coord;
            eventEntity.Status = EventStatus.NotYet.ToString();
            eventEntity.Location = request.EventRequestDto.Location.Name;
            if (request.EventRequestDto.TagId.Count > 5)
            {
                return new APIResponse
                {
                    Message = MessageEvent.TagLimitValidation,
                    StatusResponse = HttpStatusCode.BadRequest
                };
            }
            foreach (int item in request.EventRequestDto.TagId)
            {
                var tag = await _tagRepository.GetById(item);
                eventEntity.Tags.Add(tag);
            }
            await _eventRepo.Add(eventEntity);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                await _quartzService.StartEventStatusToOngoingJob(eventEntity.Id, DateTimeHelper.ToDateTime(eventEntity.StartDate));
                await _quartzService.StartEventStatusToEndedJob(eventEntity.Id, DateTimeHelper.ToDateTime(eventEntity.EndDate));
                await _quartzService.StartEventStartingEmailNoticeJob(eventEntity.Id, DateTimeHelper.ToDateTime(eventEntity.StartDate).AddHours(-1));
                await _quartzService.StartEventEndingEmailNoticeJob(eventEntity.Id, DateTimeHelper.ToDateTime(eventEntity.EndDate).AddHours(1));
                var response = _mapper.Map<EventResponseDto>(eventEntity);
                return new APIResponse
                {
                    Data = response,
                    Message = MessageCommon.CreateSuccesfully,
                    StatusResponse = HttpStatusCode.OK
                };
            }


            return new APIResponse
            {
                Message = MessageCommon.CreateFailed,
                StatusResponse = HttpStatusCode.BadRequest
            };
        }

        private bool IsValidCoordinateString(string coordinateString)
        {
            string pattern = @"^-?\d+(?:\.\d+)?, *-?\d+(?:\.\d+)?$";
            return Regex.IsMatch(coordinateString, pattern);
        }
    }
}
