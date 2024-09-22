using Application.Helper;
using Application.UseCases.Quizs.Commands.CreateQuestions;
using Application.UseCases.Quizs.Commands.CreateQuiz;
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
            if(result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("question")]
        public async Task<IActionResult> CreateQuestion([FromBody, Required] CreateQuestionDto dto, [FromQuery, Required] Guid quizId,
            CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
        }
        [Authorize]
        [HttpPost("question/multiple")]
        public async Task<IActionResult> CreateMultipleQuestion([FromBody, Required] List<CreateQuestionDto> dto, [FromQuery, Required] Guid quizId,
            CancellationToken token = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());
            var result = await _mediator.Send(new CreateQuestionsCommand(quizId, userId, dto), token);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteQuiz([FromQuery, Required] Guid id, CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
        }
        [HttpDelete("question")]
        public async Task<IActionResult> DeleteQuestion([FromQuery, Required] Guid id, CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
        }
        [HttpPut]
        public async Task<IActionResult> UpdateQuiz([FromBody, Required] Guid id, CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
        }
        [HttpPut("question")]
        public async Task<IActionResult> UpdateQuestion([FromBody, Required] Guid id, CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
        }
        [HttpGet]
        public async Task<IActionResult> GetQuizByEventId([FromQuery, Required] Guid id, CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
        }
        [HttpGet("info")]
        public async Task<IActionResult> GetQuizQuestions([FromQuery, Required] Guid quizId, CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
        }
    }
}
