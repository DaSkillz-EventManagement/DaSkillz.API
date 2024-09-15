using Application.Helper;
using Application.UseCases.Events.Command.CreateEvent;
using AutoMapper;
using Domain.DTOs.Events;
using Domain.DTOs.Events.RequestDto;
using Domain.DTOs.Events.ResponseDto;
using Domain.DTOs.Feedbacks;
using Domain.DTOs.User.Response;
using Domain.Entities;
using Domain.Models.Pagination;

namespace Application.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ReverseMap();

            CreateMap<PagedList<User>, PagedList<UserResponseDto>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items)).ReverseMap();

            CreateMap<User, UserByKeywordResponseDto>().ReverseMap();
            CreateMap<User, UserUpdatedResponseDto>().ReverseMap();

            CreateMap<Event, CreateEventCommand>().ReverseMap();
            CreateMap<Event, EventRequestDto>().ReverseMap();
            CreateMap<Event, EventDetailDto>().ReverseMap();
            CreateMap<PagedList<Event>, PagedList<EventResponseDto>>().ReverseMap();
            CreateMap<PagedList<Feedback>, PagedList<FeedbackEvent>>().ReverseMap();
            CreateMap<Event, EventResponseDto>()
            //.ForMember(dest => dest.Host,
            //           opt => opt.MapFrom(src => EventHelper.GetHostInfo((Guid)src.CreatedBy!)))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new EventLocation
            {
                Id = src.LocationId,
                Address = src.LocationAddress,
                Coord = src.LocationCoord,
                Url = src.LocationUrl
            }))
           
            .ForMember(dest => dest.eventTags, opt => opt.MapFrom(src => src.Tags)) // Mapping Tags to eventTags
            .ReverseMap()
            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.Location.Id))
            .ForMember(dest => dest.LocationAddress, opt => opt.MapFrom(src => src.Location.Address))
            .ForMember(dest => dest.LocationCoord, opt => opt.MapFrom(src => src.Location.Coord))
            .ForMember(dest => dest.LocationUrl, opt => opt.MapFrom(src => src.Location.Url))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.eventTags)); // Mapping eventTags to Tags


            CreateMap<Event, EventPreviewDto>()
                .ForMember(dest => dest.Host, opt => opt.MapFrom(src =>
                    src.CreatedBy != null ? EventHelper.GetHostInfo((Guid)src.CreatedBy) : null))
                .ReverseMap();



            CreateMap<EventTagDto, Tag>().ReverseMap();
        }
    }
}
