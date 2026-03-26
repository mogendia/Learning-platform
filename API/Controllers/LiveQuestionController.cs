using Application.DTOs.Live;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/live-sessions/{sessionId}/questions")]
    [ApiController]
    public class LiveQuestionController(ILiveQuestionService _liveQuestionService) : ControllerBase
    {
        [Authorize(Roles = "Student")]
        [HttpPost]
        public async Task<IActionResult> RaiseQuestion(Guid sessionId, [FromBody] CreateLiveQuestionDto dto)
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _liveQuestionService.RaiseQuestionAsync(sessionId, dto, studentId);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet]
        public async Task<IActionResult> GetQuestions(Guid sessionId)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _liveQuestionService.GetQuestionsAsync(sessionId, instructorId);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost("{questionId}/approve")]
        public async Task<IActionResult> Approve(Guid sessionId, Guid questionId)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _liveQuestionService.ApproveQuestionAsync(sessionId, questionId, instructorId);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost("{questionId}/revoke")]
        public async Task<IActionResult> Revoke(Guid sessionId, Guid questionId)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _liveQuestionService.RevokeQuestionAsync(sessionId, questionId, instructorId);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
