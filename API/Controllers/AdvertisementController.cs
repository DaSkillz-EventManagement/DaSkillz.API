using Application.ResponseMessage;
using Application.UseCases.Advertisement.Commands.UpdateEventAdvertisement;
using Application.UseCases.Advertisement.Queries.GetEventAdvertisement;
using Application.UseCases.Events.Queries.GetEventInfo;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers
{
    [Route("api/v1/advertisement")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private ISender _mediator;


        public AdvertisementController(ISender mediator)
        {
            _mediator = mediator;

        }

        [HttpGet("")]
        public async Task<ActionResult<APIResponse>> GetAdvertisementEvent([FromQuery, Required] GetEventAdvertisementQuery command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageEvent.GetAllEvent,
                Data = result
            });
        }

        [HttpPut("")]
        public async Task<ActionResult<APIResponse>> UpdateAdvertisementEvent([FromQuery, Required] UpdateEventAdvertisementQuery command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.UpdateSuccesfully,
                Data = result
            });
        }


    }
}
