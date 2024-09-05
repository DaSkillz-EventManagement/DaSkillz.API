using Application.UseCases.Events.Command;
using AutoMapper;
using Domain.Entities;

namespace Application.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<Event, CreateEventCommand>().ReverseMap();
        }
    }
}
