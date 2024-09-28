using Application.Helper;
using Application.UseCases.Quizs.Commands.AttempQuiz;
using Application.UseCases.Quizs.Commands.CreateQuestions;
using Application.UseCases.Quizs.Commands.CreateQuiz;
using Application.UseCases.Quizs.Commands.DeleteQuestions;
using Application.UseCases.Quizs.Commands.DeleteQuiz;
using Application.UseCases.Quizs.Commands.UpdateQuestion;
using Application.UseCases.Quizs.Commands.UpdateQuiz;
using Application.UseCases.Quizs.Queries.GetQuizByEventId;
using Application.UseCases.Quizs.Queries.GetQuizInfo;
using Domain.DTOs.Quiz.Request;
using Domain.Entities;
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

        [Authorize]
        [HttpPost("question")]
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

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteQuiz([FromQuery, Required] Guid QuizId, CancellationToken token = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());
            var result = await _mediator.Send(new DeleteQuizCommand(QuizId, userId), token);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpDelete("question")]
        public async Task<IActionResult> DeleteQuestion([FromQuery, Required] List<Guid> QuestionId, CancellationToken token = default)
        {
            var result = await _mediator.Send(new DeleteQuestionCommand(QuestionId), token);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateQuiz([FromQuery, Required] Guid QuizId, [FromBody, Required] UpdateQuizDto dto, CancellationToken token = default)
        {
            var result = await _mediator.Send(new UpdateQuizCommand(dto, QuizId), token);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }



        [Authorize]
        [HttpPut("question")]
        public async Task<IActionResult> UpdateQuestion([FromBody, Required] List<UpdateQuestionDto> dto,[FromQuery] Guid questionId, CancellationToken token = default)
        {
            var result = await _mediator.Send(new UpdateQuestionCommand(dto, questionId), token);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            return BadRequest(result);
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
            var result = await _mediator.Send(new GetQuizInfoQuery(QuizId), token);
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


        [Authorize]
        [HttpPost("/attempt")]
        public async Task<IActionResult> AttemptQuiz([FromBody, Required] List<AttempQuizDto> dtos, [FromQuery, Required] Guid quizId, [FromQuery] string totalTime,
            CancellationToken token = default)
        {
            Guid userId = Guid.Parse(User.GetUserIdFromToken());
            var result = await _mediator.Send(new AttemptQuizCommand(dtos, userId, quizId, totalTime), token);
            if (result.StatusResponse == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
