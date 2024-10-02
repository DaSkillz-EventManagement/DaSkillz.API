using Application.UseCases.Certificate.Command;
using Application.UseCases.Certificate.Queries;
using Application.UseCases.Subscriptions.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace API.Controllers
{
    public class CertificateController : Controller
    {
        private readonly IMediator _mediator;

        public CertificateController(IMediator mediator)
        {

            _mediator = mediator;
        }

        [HttpGet("filter")]
        [SwaggerOperation(Summary = "Get filtered Certificate information")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFilterCertificate(
                                                [FromQuery] Guid? userId,
                                                [FromQuery] Guid? isActive,
                                                [FromQuery] DateTime? issueDate,
                                                CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetFilterCertificate(userId, isActive, issueDate), cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }

        [HttpPost("")]
        [SwaggerOperation(Summary = "Create bulk of users'certificate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBulkCertificate([FromBody] CreateCertificateCommand command,CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }
    }
}
