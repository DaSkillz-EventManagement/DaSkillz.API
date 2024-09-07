using Application.UseCases.Events.Command;
using AutoMapper;
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
           
        }
    }
}
