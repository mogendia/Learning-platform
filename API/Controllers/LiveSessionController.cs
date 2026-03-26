using Application.DTOs.Live;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveSessionController(ILiveSessionService _liveSessionService) : ControllerBase
    {
        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody] CreateLiveSessionDto dto)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _liveSessionService.CreateSessionAsync(dto, instructorId);
            if (!result.Success)
                return BadRequest(result);
            return CreatedAtAction(nameof(GetSession), new { sessionId = result.Data.SessionId }, result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost("{sessionId}/start")]
        public async Task<IActionResult> StartSession(Guid sessionId)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _liveSessionService.StartSessionAsync(sessionId, instructorId);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost("{sessionId}/end")]
        public async Task<IActionResult> EndSession(Guid sessionId)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _liveSessionService.EndSessionAsync(sessionId, instructorId);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetSession(Guid sessionId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isInstructor = User.IsInRole("Instructor");
            var result = await _liveSessionService.GetSessionAsync(sessionId, userId, isInstructor);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("{sessionId}/join")]
        public async Task<IActionResult> JoinSession(Guid sessionId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isInstructor = User.IsInRole("Instructor");
            var result = await _liveSessionService.JoinSessionAsync(sessionId, userId, isInstructor);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetActiveSessionByCourse(int courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isInstructor = User.IsInRole("Instructor");
            var result = await _liveSessionService.GetActiveSessionByCourseAsync(courseId, userId, isInstructor);

            if (!result.Success)
            {
                if (result.Message == "No active session")
                    return NotFound(result);

                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
