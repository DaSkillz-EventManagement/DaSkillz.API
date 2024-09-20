using Application.UseCases.Coupons.Command.UpdateCoupon;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Coupons.Command.DeleteCoupon
{
    public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICouponRepository _couponRepository;
        private readonly IEventRepository _eventRepository;

        public DeleteCouponCommandHandler(IUnitOfWork unitOfWork, ICouponRepository couponRepository, IEventRepository eventRepository)
        {
            _unitOfWork = unitOfWork;
            _couponRepository = couponRepository;
            _eventRepository = eventRepository;
        }

        public async Task<bool> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
        {
            var isOwner = await _eventRepository.IsOwner(request.CouponEventDto.UserId, request.CouponEventDto.EventId);
            if (isOwner)
            {
                var result = _couponRepository.Delete(request.Id);
                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }
    }
}
