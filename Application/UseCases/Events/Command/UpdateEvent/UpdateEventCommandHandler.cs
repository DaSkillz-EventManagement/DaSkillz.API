using Application.ExternalServices.Images;
using Application.ExternalServices.Quartz;
using Application.Helper;
using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Events;
using Domain.DTOs.Events.ResponseDto;
using Domain.Entities;
using Domain.Enum.Events;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Events.Command.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, APIResponse>
    {
        private readonly IEventRepository _eventRepo;
        private readonly ITagRepository _tagRepository;
        private readonly IQuartzService _quartzService;
        private readonly IImageService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly long minimumUpdateTimeSpan = 21600000;

        public UpdateEventCommandHandler(IEventRepository eventRepo, ITagRepository tagRepository, IQuartzService quartzService, IImageService fileService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _tagRepository = tagRepository;
            _quartzService = quartzService;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await _eventRepo.GetById(request.EventId);
            if (eventEntity!.Status != EventStatus.NotYet.ToString())
            {
                return new APIResponse
                {
                    Message = MessageEvent.UpdateEventWithStatus,
                    StatusResponse = HttpStatusCode.BadRequest,
                };
            }
            if (request.UserId == null)
            {
                return new APIResponse
                {
                    Message = MessageCommon.Unauthorized,
                    StatusResponse = HttpStatusCode.Unauthorized,
                };
            }
            if (!request.UserId.Equals(eventEntity.CreatedBy!.Value))
            {
                return new APIResponse
                {
                    Message = MessageEvent.OnlyHostCanUpdateEvent,
                    StatusResponse = HttpStatusCode.Unauthorized,
                };
            }
            eventEntity.UpdatedAt = DateTimeHelper.ToJsDateType(DateTimeHelper.GetDateTimeNow());
            //name
            eventEntity.EventName = request.EventRequestDto.EventName;
            //description
            eventEntity.Description = request.EventRequestDto.Description;
            //tags;
            eventEntity.Tags.Clear();
            foreach (int tagId in request.EventRequestDto.TagId)
            {
                Tag tag = await _tagRepository.GetById(tagId);
                eventEntity.Tags.Add(tag!);
            }
            //startDate
            if (request.EventRequestDto.StartDate > 0 && request.EventRequestDto.StartDate - eventEntity.CreatedAt! < minimumUpdateTimeSpan)
            {
                return new APIResponse
                {
                    Message = MessageEvent.UpdateStartEndTimeValidation,
                    StatusResponse = HttpStatusCode.BadRequest,
                };
            }
            if (request.EventRequestDto.StartDate > 0 && request.EventRequestDto.StartDate - eventEntity.CreatedAt! >= minimumUpdateTimeSpan)
            {
                eventEntity.StartDate = request.EventRequestDto.StartDate;
                await _quartzService.DeleteJobsByEventId("start-" + eventEntity.EventId);
                await _quartzService.StartEventStatusToOngoingJob(eventEntity.EventId, DateTimeHelper.ToDateTime(eventEntity.StartDate));
                await _quartzService.StartEventStartingEmailNoticeJob(eventEntity.EventId, DateTimeHelper.ToDateTime(eventEntity.StartDate).AddHours(-1));
            }
            //endDate
            if (request.EventRequestDto.EndDate > 0 && request.EventRequestDto.EndDate - eventEntity.StartDate < 30 * 60 * 1000)
            {
                return new APIResponse
                {
                    Message = MessageEvent.UpdateStartEndTimeValidation,
                    StatusResponse = HttpStatusCode.BadRequest,
                };
            }
            if (request.EventRequestDto.EndDate > 0 && request.EventRequestDto.EndDate - eventEntity.StartDate >= 30 * 60 * 1000)
            {
                eventEntity.EndDate = request.EventRequestDto.EndDate;
                await _quartzService.DeleteJobsByEventId("ended-" + eventEntity.EventId);
                await _quartzService.StartEventStatusToEndedJob(eventEntity.EventId, DateTimeHelper.ToDateTime(eventEntity.EndDate));
                await _quartzService.StartEventEndingEmailNoticeJob(eventEntity.EventId, DateTimeHelper.ToDateTime(eventEntity.EndDate).AddHours(1));
            }
            //theme
            eventEntity.Theme = request.EventRequestDto.Theme;
            //image
            if (!string.IsNullOrWhiteSpace(request.EventRequestDto.Image))
            {
                string url = eventEntity.Image!;
                int startIndex = url.LastIndexOf("/eventcontainer/") + "/eventcontainer/".Length;
                string result = url.Substring(startIndex);
                if (await _fileService.DeleteBlob(result))
                    eventEntity.Image = await _fileService.UploadImage(request.EventRequestDto.Image, Guid.NewGuid());
            }
            //location
            eventEntity.Location = request.EventRequestDto.Location!.Name;
            eventEntity.LocationId = request.EventRequestDto.Location.Id;
            eventEntity.LocationCoord = request.EventRequestDto.Location.Coord;
            eventEntity.LocationAddress = request.EventRequestDto.Location.Address;
            eventEntity.LocationUrl = request.EventRequestDto.Location.Url;
            //capacity
            eventEntity.Capacity = request.EventRequestDto.Capacity.HasValue ? request.EventRequestDto.Capacity.Value : eventEntity.Capacity;
            //approval
            eventEntity.Approval = request.EventRequestDto.Approval.HasValue ? request.EventRequestDto.Approval.Value : eventEntity.Approval;
            //fare / ticket
            eventEntity.Fare = request.EventRequestDto.Ticket.HasValue ? request.EventRequestDto.Ticket.Value : eventEntity.Fare;
            await _eventRepo.Update(eventEntity);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                var eventResponse = _mapper.Map<EventResponseDto>(eventEntity);
                eventResponse.UpdatedAt = eventEntity.UpdatedAt.HasValue ? eventEntity.UpdatedAt.Value : null;
                eventResponse.eventTags = _mapper.Map<List<EventTagDto>>(eventEntity.Tags);
                return new APIResponse
                {
                    Message = MessageCommon.UpdateSuccesfully,
                    StatusResponse = HttpStatusCode.OK,
                    Data = eventResponse
                };
            }
            return new APIResponse
            {
                Message = MessageCommon.UpdateFailed,
                StatusResponse = HttpStatusCode.BadRequest,
            };
        }
    }
}
