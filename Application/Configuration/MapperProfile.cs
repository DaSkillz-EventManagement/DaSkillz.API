using Application.Helper;
using Application.UseCases.Events.Command.CreateEvent;
using AutoMapper;
using Domain.DTOs.Events;
using Domain.Entities;
using Domain.Models.Pagination;

namespace Application.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<Event, CreateEventCommand>().ReverseMap();
            CreateMap<Event, EventDetailDto>().ReverseMap();
            CreateMap<PagedList<Event>, PagedList<EventResponseDto>>().ReverseMap();
            CreateMap<Event, EventPreviewDto>()
                .ForMember(dest => dest.Host, opt => opt.MapFrom(src => EventHelper.GetHostInfo((Guid)src.CreatedBy!)))
                .ReverseMap();
        }
    }
}
