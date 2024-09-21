using Application.Helper;
using Application.ResponseMessage;
using Application.UseCases.Coupons.Queries.GetCoupon;
using AutoMapper;
using Domain.DTOs.Coupons;
using Domain.Entities;
using Domain.Models.Pagination;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Elastic.Clients.Elasticsearch;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Coupons.Command.CreateCoupon
{
    public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommand, APIResponse>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventRepository _eventRepository;

        public CreateCouponCommandHandler(ICouponRepository couponRepository, IMapper mapper, IUnitOfWork unitOfWork, IEventRepository eventRepository)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _eventRepository = eventRepository;
        }

        public async Task<APIResponse> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
        {
            var isOwner = await _eventRepository.IsOwner(request.CouponEventDto.UserId, request.CouponEventDto.EventId);
            if (isOwner)
            {
                var couponEntity = _mapper.Map<Coupon>(request.Coupon);
                couponEntity.CreatedDate = DateTimeHelper.GetCurrentTimeAsLong();
                couponEntity.Id = Guid.NewGuid().ToString();
                var response = _couponRepository.Add(couponEntity);
                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    return new APIResponse()
                    {
                        StatusResponse = HttpStatusCode.OK,
                        Message = MessageCommon.CreateSuccesfully,
                        Data = couponEntity,
                    };
                }
                else
                {
                    return new APIResponse()
                    {
                        StatusResponse = HttpStatusCode.BadRequest,
                        Message = MessageCommon.CreateFailed,
                        Data = null,
                    };
                }
            }
            else
            {
                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageEvent.OnlyHostCanUpdateEvent,
                    Data = null,
                };
            }
            
                
        }
    }
}
