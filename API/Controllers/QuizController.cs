using Application.Helper;
using Application.UseCases.Quizs.Commands.CreateQuestions;
using Application.UseCases.Quizs.Commands.CreateQuiz;
using Application.UseCases.Quizs.Queries.GetQuizByEventId;
using Domain.DTOs.Quiz.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers
{
    [Route("api/v1/quiz")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private ISender _mediator;


        public QuizController(ISender mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody, Required] CreateQuizDto dto, CancellationToken token = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());
            var result = await _mediator.Send(new CreateQuizCommand(userId, dto), token);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("question")]
        public async Task<IActionResult> CreateQuestion([FromBody, Required] CreateQuestionDto dto, [FromQuery, Required] Guid QuizId,
            CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
        }
        [Authorize]
        [HttpPost("question/multiple")]
        public async Task<IActionResult> CreateMultipleQuestion([FromBody, Required] List<CreateQuestionDto> dto, [FromQuery, Required] Guid QuizId,
            CancellationToken token = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());
            var result = await _mediator.Send(new CreateQuestionsCommand(QuizId, userId, dto), token);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteQuiz([FromQuery, Required] Guid QuizId, CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
        }
        [HttpDelete("question")]
        public async Task<IActionResult> DeleteQuestion([FromQuery, Required] Guid QuestionId, CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
        }
        [HttpPut]
        public async Task<IActionResult> UpdateQuiz([FromBody, Required] Guid QuizId, CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
        }
        [HttpPut("question")]
        public async Task<IActionResult> UpdateQuestion([FromBody, Required] Guid QuestionId, CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
        }
        [HttpGet]
        public async Task<IActionResult> GetQuizByEventId([FromQuery, Required] Guid EventId, CancellationToken token = default)
        {
            var result = await _mediator.Send(new GetQuizByEventIdQuery(EventId), token);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        [HttpGet("info")]
        public async Task<IActionResult> GetQuizQuestions([FromQuery, Required] Guid QuizId, CancellationToken token = default)
        {
            var result = await _mediator.Send(new GetQuizByEventIdQuery(QuizId), token);
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

        [HttpPost("/attemp")]
        public async Task<IActionResult> AttempQuiz([FromQuery, Required] Guid quizid, [FromQuery, Required] Guid eventid, CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
        }
    }
}
