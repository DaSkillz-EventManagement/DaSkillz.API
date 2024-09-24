using Application.UseCases.Tags.Commands.AddTag;
using Application.UseCases.Tags.Commands.DeleteTag;
using Application.UseCases.Tags.Queries.GetAllTag;
using Application.UseCases.Tags.Queries.GetById;
using Application.UseCases.Tags.Queries.GetTrendingTags;
using Application.UseCases.Tags.Queries.SearchTag;
using Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers
{
    [Route("api/v1/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTag([FromQuery, Range(1, int.MaxValue)] int page = 1, [FromQuery, Range(1, int.MaxValue)] int eachPage = 10, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllTagQuery(page, eachPage), cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddTag([FromBody] AddTagCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }

        [HttpDelete("")]
        public async Task<IActionResult> DeleteTag([FromQuery] DeleteTagCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }


        [HttpGet("keyword")]
        public async Task<IActionResult> SearchByKeyWord([FromQuery] SearchTagQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetTagById([FromQuery] GetTagByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }

        [HttpGet("trending")]
        public async Task<IActionResult> TrendingTask(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetTrendingTagQuery(), cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }


    }
}
