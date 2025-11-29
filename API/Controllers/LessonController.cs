using Application.DTOs.Lesson;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LessonController(ILessonService _lessonService, ICourseService _courseService) : ControllerBase
    {
        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public async Task<IActionResult> CreateLesson([FromForm] CreateLessonDto dto)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _lessonService.CreateLessonAsync(dto, instructorId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLessons()
        {
            var result = await _lessonService.GetAllLessonsAsync();
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLessonDetails(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _lessonService.GetLessonDetailsAsync(id, userId);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _lessonService.DeleteLessonAsync(id, instructorId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Student")]
        [HttpPost("{id}/progress")]
        public async Task<IActionResult> UpdateProgress(int id, [FromBody] int watchedSeconds)
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _lessonService.UpdateProgressAsync(id, studentId, watchedSeconds);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetLessonsByCourse(int courseId)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verify the instructor owns the course
            var lessons = await _lessonService.GetLessonsByCourseIdAsync(courseId, instructorId);

            if (!lessons.Success)
                return Unauthorized(lessons);

            return Ok(lessons);
        }
    }

}
