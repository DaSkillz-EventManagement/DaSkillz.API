using Application.Helper;
using Application.ResponseMessage;
using Application.UseCases.Events.Command.CreateEvent;
using Domain.Entities;
using Domain.Enum.Events;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Command.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, APIResponse>
    {
        private readonly IEventRepository _eventRepo;
        private readonly ITagRepository _tagRepository;
        private readonly long dateTimeConvertValue = 25200000; //-7h to match JS dateTime type
        private readonly long minimumUpdateTimeSpan = 21600000;

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
            if (!request.UserId.Equals(eventEntity.CreatedBy!.Value.ToString()))
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
                eventEntity.StartDate = request.EventRequestDto.StartDate + dateTimeConvertValue;
                await _quartzService.DeleteJobsByEventId("start-" + eventEntity.Id);
                await _quartzService.StartEventStatusToOngoingJob(eventEntity.Id, eventEntity.StartDate);
                await _quartzService.StartEventStartingEmailNoticeJob(eventEntity.Id, eventEntity.StartDate.AddHours(-1));
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
                eventEntity.EndDate = request.EventRequestDto.EndDate + dateTimeConvertValue;
                await _quartzService.DeleteJobsByEventId("ended-" + eventEntity.Id);
                await _quartzService.StartEventStatusToEndedJob(eventEntity.Id, eventEntity.EndDate);
                await _quartzService.StartEventEndingEmailNoticeJob(eventEntity.Id, eventEntity.EndDate.AddHours(1));
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
            eventEntity.Location = eventDto.Location!.Name;
            eventEntity.LocationId = eventDto.Location.Id;
            eventEntity.LocationCoord = eventDto.Location.Coord;
            eventEntity.LocationAddress = eventDto.Location.Address;
            eventEntity.LocationUrl = eventDto.Location.Url;
            //capacity
            eventEntity.Capacity = eventDto.Capacity.HasValue ? eventDto.Capacity.Value : eventEntity.Capacity;
            //approval
            eventEntity.Approval = eventDto.Approval.HasValue ? eventDto.Approval.Value : eventEntity.Approval;
            //fare / ticket
            eventEntity.Fare = eventDto.Ticket.HasValue ? eventDto.Ticket.Value : eventEntity.Fare;
            await _unitOfWork.EventRepository.Update(eventEntity);
            if (await _unitOfWork.SaveChangesAsync())
            {
                return new APIResponse
                {
                    Message = MessageCommon.UpdateSuccesfully,
                    StatusResponse = HttpStatusCode.OK,
                    Data = ToResponseDto(eventEntity)
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
