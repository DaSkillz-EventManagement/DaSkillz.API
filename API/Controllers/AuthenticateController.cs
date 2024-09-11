using Application.Helper;
using Application.UseCases.Authenticate.Commands.SignInWithGoogle;
using Application.UseCases.Authenticate.Commands.SignInWIthOtp;
using Application.UseCases.Authenticate.Commands.SignOut;
using Application.UseCases.Authenticate.Commands.SignUpWithOtp;
using Application.UseCases.Authenticate.Queries.RefreshToken;
using Application.UseCases.Authenticate.Queries.ValidateOTP;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Event_Management.API.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthenticateController : Controller
    {
        private ISender _mediator;
        public AuthenticateController(ISender mediator)
        {
            _mediator = mediator;

        }


        [HttpPost("sign-in/google")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignInGoogle([FromBody] SignInGoogleCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }


        [HttpPost("sign-in/otp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignInOtp([FromBody] SignInOtpCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }


        [HttpPost("sign-up/otp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }


        [HttpPost("otp/validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidateOtp([FromBody] ValidateOtpQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }

        [Authorize]
        [HttpPost("sign-out")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignOut(CancellationToken cancellationToken)
        {
            var userId = User.GetUserIdFromToken();
            var result = await _mediator.Send(new SignOutCommand(userId), cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }

    }
}
