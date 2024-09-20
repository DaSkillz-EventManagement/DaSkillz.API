using Domain.DTOs.Quiz.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody, Required] CreateQuizDto dto, CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
        }
        [HttpPost("question")]
        public async Task<IActionResult> CreateQuestion([FromBody, Required] CreateQuestionDto dto, CancellationToken token = default)
        {
            return Ok(new NotImplementedException());
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
