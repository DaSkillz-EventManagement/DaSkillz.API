using Application.UseCases.Events.Command.CreateEvent;
using AutoMapper;
using Domain.DTOs.Coupons;
using Domain.DTOs.Events;
using Domain.DTOs.Events.RequestDto;
using Domain.DTOs.Events.ResponseDto;
using Domain.DTOs.Feedbacks;
using Domain.DTOs.Payment.Response;
using Domain.DTOs.PriceDto;
using Domain.DTOs.Tag;
using Domain.DTOs.User.Response;
using Domain.Entities;
using Domain.Models.Pagination;
using Domain.DTOs.PriceDto;
using Domain.DTOs.Coupons;
using Domain.DTOs.AdvertisedEvents;

namespace Application.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Transaction, TransactionResponseDto>()
                .ReverseMap();

            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dest => dest.IsPremiumUser, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.IsPremiumUser = src.Subscription.IsActive;
                })
                .ReverseMap();
            
            CreateMap<User, UserUpdatedResponseDto>()
                .ForMember(dest => dest.IsPremiumUser, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.IsPremiumUser = src.Subscription.IsActive;
                })
                .ReverseMap();

            CreateMap<User, UserByKeywordResponseDto>()
                .ForMember(dest => dest.IsPremiumUser, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.IsPremiumUser = src.Subscription.IsActive;
                })
                .ReverseMap();


            CreateMap<PagedList<User>, PagedList<UserResponseDto>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items)).ReverseMap();

            CreateMap<User, UserByKeywordResponseDto>().ReverseMap();
            CreateMap<User, UserUpdatedResponseDto>().ReverseMap();
            CreateMap<User, UserResponseDto>().ReverseMap();

            CreateMap<Event, CreateEventCommand>().ReverseMap();
            CreateMap<Event, EventRequestDto>().ReverseMap();
            CreateMap<Event, EventDetailDto>()
                    .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new EventLocation
                    {
                        Id = src.LocationId,
                        Address = src.LocationAddress,
                        Coord = src.LocationCoord,
                        Url = src.LocationUrl
                    }))

                    .ForMember(dest => dest.eventTags, opt => opt.MapFrom(src => src.Tags)) // Mapping Tags to eventTags
                    .ReverseMap()
                    .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.Name))
                    .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.Location.Id))
                    .ForMember(dest => dest.LocationAddress, opt => opt.MapFrom(src => src.Location.Address))
                    .ForMember(dest => dest.LocationCoord, opt => opt.MapFrom(src => src.Location.Coord))
                    .ForMember(dest => dest.LocationUrl, opt => opt.MapFrom(src => src.Location.Url))
                    .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.eventTags)) // Mapping eventTags to Tags
                    .ReverseMap();
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
            .ForMember(dest => dest.Host, opt => opt.MapFrom(src => src.CreatedByNavigation))
            .ForMember(dest => dest.eventTags, opt => opt.MapFrom(src => src.Tags)) // Mapping Tags to eventTags
            .ReverseMap()
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.Name))
            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.Location.Id))
            .ForMember(dest => dest.LocationAddress, opt => opt.MapFrom(src => src.Location.Address))
            .ForMember(dest => dest.LocationCoord, opt => opt.MapFrom(src => src.Location.Coord))
            .ForMember(dest => dest.LocationUrl, opt => opt.MapFrom(src => src.Location.Url))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.eventTags)); // Mapping eventTags to Tags


            CreateMap<Event, EventPreviewDto>()
               .ForMember(dest => dest.Host, opt => opt.MapFrom(src => src.CreatedByNavigation)) // Map CreatedByNavigation to Host
                .ReverseMap();
            // Mapping from User (or whatever class represents the user) to CreatedByUserDto
            CreateMap<User, CreatedByUserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.avatar, opt => opt.MapFrom(src => src.Avatar)); // Assuming User entity has these fields


            CreateMap<CouponDto, Coupon>().ReverseMap();
            CreateMap<PriceDto, Price>().ReverseMap();
            CreateMap<Price, ResponsePriceDto>();
            //.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => EventHelper.GetHostInfo(src.CreatedBy)));

            //Mapper Tag
            CreateMap<TagDto, Tag>().ReverseMap();
            CreateMap<PagedList<TagDto>, PagedList<Tag>>().ReverseMap();
            CreateMap<EventTagDto, Tag>().ReverseMap();
            CreateMap<AdvertisedEvent, AdvertisedEventDto>();
        }
    }
}
