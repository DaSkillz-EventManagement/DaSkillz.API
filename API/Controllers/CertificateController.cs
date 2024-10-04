using Application.UseCases.Certificate.Command;
using Application.UseCases.Certificate.Queries.GetFilterCertificate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace API.Controllers
{
    [Route("api/v1/certificate")]
    [ApiController]
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
                                                [FromQuery] Guid? certificateId,
                                                [FromQuery] Guid? userId,
                                                [FromQuery] Guid? eventId,
                                                [FromQuery] DateTime? issueDate,
                                                CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetFilterCertificate(certificateId, userId, eventId, issueDate), cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }

        [Authorize]
        [HttpPost("")]
        [SwaggerOperation(Summary = "Create bulk of users'certificate", Description = "{\r\n  \"eventId\": \"aa00c37f-b734-447b-8c81-e70822c73104\",\r\n  \"userIds\": [\r\n    \"a4593c8c-6f8c-4244-960e-fc304a5dfe4e\",\r\n    \"2ac2a6f4-19c1-4380-afce-a577689fa8e4\"\r\n  ]\r\n}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBulkCertificate([FromBody] CreateCertificateCommand command,CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }
    }
}
