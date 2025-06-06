﻿using Application.UseCases.Events.Command.CreateEvent;
using AutoMapper;
using Domain.DTOs.AdvertisedEvents;
using Domain.DTOs.Certificate;
using Domain.DTOs.Coupons;
using Domain.DTOs.Events;
using Domain.DTOs.Events.RequestDto;
using Domain.DTOs.Events.ResponseDto;
using Domain.DTOs.Feedbacks;
using Domain.DTOs.ParticipantDto;
using Domain.DTOs.Payment.Response;
using Domain.DTOs.PriceDto;
using Domain.DTOs.Quiz.Request;
using Domain.DTOs.Quiz.Response;
using Domain.DTOs.Sponsors;
using Domain.DTOs.Subscription;
using Domain.DTOs.Tag;
using Domain.DTOs.User.Response;
using Domain.Entities;
using Domain.Models.Pagination;

namespace Application.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Transaction, MinimalTransactionResponseDto>()
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.EventName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ReverseMap();

            CreateMap<Transaction, TransactionResponseDto>()
                .ForMember(dest => dest.OrderUrl, opt => opt.MapFrom(src => src.Status == 3 ? src.OrderUrl : null))
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.EventName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ReverseMap();

            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ReverseMap();
            CreateMap<User, AttemptedQuizUserResponse>();
            CreateMap<PagedList<User>, PagedList<UserResponseDto>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items)).ReverseMap();


            CreateMap<User, UserByKeywordResponseDto>().ReverseMap();
            CreateMap<User, UserUpdatedResponseDto>().ReverseMap();

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

            //Mapper Feedback
            CreateMap<FeedbackDto, Feedback>().ReverseMap();
            CreateMap<Feedback, FeedbackEvent>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar))
                .ReverseMap();
            CreateMap<PagedList<Feedback>, PagedList<FeedbackEvent>>();


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
            CreateMap<AdvertisedEvent, AdvertisedEventDto>().ReverseMap();
            //.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => EventHelper.GetHostInfo(src.CreatedBy)));


            //Mapper Quiz
            CreateMap<CreateQuizDto, Quiz>().ReverseMap();
            CreateMap<UserAnswer, UserAnswerResponseDto>().ReverseMap();
            CreateMap<Quiz, ResponseQuizDto>();
            CreateMap<Answer, ResponseAnswerDto>();
            CreateMap<Answer, ResponseAnswerQuizAttemptDto>();
            CreateMap<Question, UserAnswerResultDto>();
            CreateMap<Question, ResponseQuestionDto>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(dest => dest.Answers)).ReverseMap();

            CreateMap<Question, ResponseQuizAttempt>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(dest => dest.Answers)).ReverseMap();
            CreateMap<UpdateQuizDto, Quiz>().ReverseMap();
            //sponsor
            CreateMap<SponsorEvent, SponsorEventDto>();
            //CreateMap<List<SponsorEvent>, List<SponsorEventDto>>().ReverseMap();
            CreateMap<SponsorEvent, SponsorEventDetailDto>();


            //
            CreateMap<Certificate, CertificateResponseDto>()
                .ForMember(dest => dest.certicateId, opt => opt.MapFrom(src => src.CertificateID))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Participant!.User.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Participant!.User.Email))
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Participant.Event.EventName))
                .ReverseMap();

            CreateMap<Participant, FilterCertificateDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Certificate, opt => opt.MapFrom(src => src.Certificates.FirstOrDefault())) // Map the first certificate
                .ReverseMap();



            //Participant
            CreateMap<Participant, ParticipantEventDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                //.ForMember(dest => dest.certification, opt => opt.MapFrom(src =>
                //            src.Certificates.FirstOrDefault(c => c.EventId == src.EventId)))
                .ReverseMap();

            CreateMap<PagedList<Participant>, PagedList<ParticipantEventDto>>().ReverseMap();
            CreateMap<Participant, ParticipantInfo>().ReverseMap();

            CreateMap<Participant, ParticipantDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ReverseMap();
            CreateMap<PagedList<Participant>, PagedList<ParticipantDto>>().ReverseMap();

            CreateMap<Subscription, SubscriptionResponseDto>().ReverseMap();

            

        }
    }
}
