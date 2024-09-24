using Application.Helper;
using Application.UseCases.Prices.Commands.Create;
using Application.UseCases.Prices.Commands.Delete;
using Application.UseCases.Prices.Commands.Update;
using Application.UseCases.Prices.Queries.GetAll;
using Application.UseCases.Prices.Queries.GetById;
using Domain.DTOs.PriceDto;
using Domain.Enum.Price;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/v1/admin/price")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private ISender _mediator;


        public PriceController(ISender mediator)
        {
            _mediator = mediator;

        }
        [HttpPost("")]
        public async Task<IActionResult> CreatePrice([FromBody, Required] PriceDto price, CancellationToken token = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());
            var result = await _mediator.Send(new CreatePriceCommand(userId, price), token);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut("")]
        public async Task<IActionResult> UpdatePrice([FromBody, Required] UpdatePriceDto price, CancellationToken token = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());
            var result = await _mediator.Send(new UpdatePriceCommand(price), token);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            if (result.StatusResponse == HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("")]
        public async Task<IActionResult> RemovePrice([FromQuery] int id, CancellationToken token = default)
        {
            var result = await _mediator.Send(new DeletePriceCommand(id), token);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            if (result.StatusResponse == HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAllPrice([FromQuery] GetAllPriceOrderBy orderBy, bool isAscending = true, CancellationToken token = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());
            var result = await _mediator.Send(new GetAllPriceQuery(orderBy, isAscending), token);
            return Ok(result);
        }
        [HttpGet("info")]
        public async Task<IActionResult> GetPrice([FromQuery] int priceId, CancellationToken token = default)
        {
            var result = await _mediator.Send(new GetPriceByIdQuery(priceId), token);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
