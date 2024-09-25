using Application.Helper;
using Application.ResponseMessage;
using Application.UseCases.Coupons.Command.CreateCoupon;
using Application.UseCases.Coupons.Command.DeleteCoupon;
using Application.UseCases.Coupons.Command.UpdateCoupon;
using Application.UseCases.Coupons.Command.UseCoupon;
using Application.UseCases.Coupons.Queries.GetCoupon;
using Application.UseCases.Coupons.Queries.GetUsersByCoupon;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers
{
    [Route("api/v1/coupon")]
    [ApiController]
    public class CouponController : ControllerBase
    {

        private ISender _mediator;


        public CouponController(ISender mediator)
        {
            _mediator = mediator;

        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAllCoupons([FromQuery, Required] GetCouponQuery query, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);

            return new APIResponse()
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.GetSuccesfully,
                Data = result
            };
        }

        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateCoupon([FromBody, Required] CreateCouponCommand command, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken();
            command.CouponEventDto.UserId = Guid.Parse(userId);
            var result = await _mediator.Send(command, cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);

        }



        [HttpPut("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateCoupon([FromBody, Required] UpdateCouponCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);

        }

        [HttpDelete("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteCoupon([FromQuery, Required] DeleteCouponCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result)
            {
                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.DeleteSuccessfully,

                };
            }
            else
            {
                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageCommon.DeleteFailed,
                    Data = null
                };
            }



        }

        [HttpPost("use-coupon")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UseCoupon([FromBody, Required] UseCouponCommand command, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken();
            command.CouponEventDto.UserId = Guid.Parse(userId);
            var result = await _mediator.Send(command, cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);

        }


        [HttpGet("user-coupon")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetUsersByCoupon([FromQuery, Required] GetUsersByCouponQuery query, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);

        }

    }
}
