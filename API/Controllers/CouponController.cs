using Application.ResponseMessage;
using Application.UseCases.Coupons.Command.CreateCoupon;
using Application.UseCases.Coupons.Command.DeleteCoupon;
using Application.UseCases.Coupons.Command.UpdateCoupon;
using Application.UseCases.Coupons.Queries.GetCoupon;
using Application.UseCases.Events.Queries.GetEventInfo;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateCoupon([FromQuery, Required] CreateCouponCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);

        }


        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateCoupon([FromQuery, Required] UpdateCouponCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return (result.StatusResponse != HttpStatusCode.OK) ? result : StatusCode((int)result.StatusResponse, result);

        }

        [HttpDelete("")]
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

    }
}
