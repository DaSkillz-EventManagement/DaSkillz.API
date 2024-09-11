using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Events;
using Domain.Enum.Events;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Events.Queries.GetEventInfo
{
    public class GetEventInfoQueryHandler : IRequestHandler<GetEventInfoQuery, APIResponse>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetEventInfoQueryHandler(IEventRepository eventRepo, IMapper mapper, IUserRepository userRepository)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<APIResponse> Handle(GetEventInfoQuery request, CancellationToken cancellationToken)
        {
            var eventInfo = await _eventRepo.GetById(request);

            if (eventInfo!.Status!.Equals(EventStatus.Deleted.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return new APIResponse
                {
                    Message = MessageCommon.NotFound,
                    StatusResponse = HttpStatusCode.NotFound,
                };
            }
            if (eventInfo != null)
            {
                var eventDetailDto = _mapper.Map<EventDetailDto>(eventInfo);
                var user = _userRepository.GetUserById((Guid)eventInfo.CreatedBy!);

                eventDetailDto.Host!.Name = user!.FullName;
                eventDetailDto.Host.avatar = user!.Avatar;
                eventDetailDto.Host.email = user!.Email!;
                eventDetailDto.Host.Id = eventInfo.CreatedBy.HasValue ? eventInfo.CreatedBy.Value : null;
                eventDetailDto.Approval = eventInfo.Approval ? eventInfo.Approval : false;
                eventDetailDto.Capacity = eventInfo.Capacity.HasValue ? eventInfo.Capacity.Value : 0;
                eventDetailDto.UpdatedAt = eventInfo.UpdatedAt.HasValue ? eventInfo.UpdatedAt.Value : null;

                eventDetailDto.eventTags = _mapper.Map<List<EventTagDto>>(eventInfo.Tags);
                return new APIResponse
                {
                    Message = MessageCommon.Complete,
                    StatusResponse = HttpStatusCode.OK,
                    Data = eventDetailDto
                };
            }
            return new APIResponse
            {
                Message = MessageCommon.NotFound,
                StatusResponse = HttpStatusCode.BadRequest,
            };

        }

    }
}
