using Application.UseCases.Payment.Commands.Callback;
using Application.UseCases.Payment.Commands.CreatePayment;
using Application.UseCases.Payment.Commands.Refund;
using Application.UseCases.Payment.Queries.GetAllTransaction;
using Application.UseCases.Payment.Queries.GetFilterTransaction;
using Application.UseCases.Payment.Queries.GetOrderStatus;
using Application.UseCases.Payment.Queries.GetTotalTransaction;
using Application.UseCases.Payment.Queries.GetTransactionByEvent;
using Application.UseCases.Payment.Queries.GetTransactionByUser;
using Domain.Enum.Payment;
using Elastic.Clients.Elasticsearch.Fluent;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers
{
    [Route("api/v1/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {

            _mediator = mediator;
        }

        [HttpGet("query-status")]
        [SwaggerOperation(Summary = "Get transaction status", Description = "Retrieves the status of a transaction based on a query parameter.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> queryTransaction([FromQuery] string query, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetOrderStatusQuery(query), cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }

        [HttpGet("")]
        [SwaggerOperation(Summary = "Get all transactions", Description = "Fetches all transactions available in the system.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllTransactionQuery(), cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }

        [HttpGet("user")]
        [SwaggerOperation(Summary = "Get transactions by user", Description = "Retrieves transactions associated with a specific user.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTransactionByUser([FromQuery] Guid guid, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetTransactionByUserQuery(guid), cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }

        [HttpGet("event")]
        [SwaggerOperation(Summary = "Get transactions by event", Description = "Retrieves transactions associated with a specific event.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventTransaction([FromQuery] Guid guid, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetEventTransactionQuery(guid), cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }


        [HttpGet("filter")]
        [SwaggerOperation(Summary = "Get filtered transactions", Description = "Status: 1=Success, 2=Fail, 3=Processing //// Subscription Type: 1=Ticket, 2=Sponsor, 3=Advertise, 4=Subscription.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFilterTransaction(
                                                [FromQuery] Guid? eventId,
                                                [FromQuery] Guid? userId,
                                                [FromQuery] TransactionStatus? status,
                                                [FromQuery] PaymentType? subscriptionType,
                                                CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetFilterTransactionQuery(eventId, userId, (int?)status, (int?)subscriptionType), cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }

        [HttpGet("total")]
        [SwaggerOperation(Summary = "Get total transactions by userId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTotalTransaction(
                                                [FromQuery] Guid? eventId,
                                                [FromQuery, Required] Guid? userId,
                                                CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetTotalTransactionQuery(eventId, userId), cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }



        [HttpPost("")]
        [SwaggerOperation(Summary = "Create a new transaction", Description = "create a new transaction (if isSubscription is true then eventId will be ignored")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTransaction([FromBody] CreatePayment command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }



        [HttpPost("callback")]
        [SwaggerOperation(Summary = "Handle payment callback", Description = "Processes the callback from the payment gateway. (for zalopay)")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<object> Callback([FromBody] CallbackCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result;
        }

        [HttpPost("refund")]
        [SwaggerOperation(Summary = "Refund a transaction", Description = "Processes a refund for a payment transaction.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<object> Callback([FromBody] RefundCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.StatusResponse != HttpStatusCode.OK ? StatusCode((int)result.StatusResponse, result) : Ok(result);
        }


    }
}
