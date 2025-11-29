using Application.DTOs.Course;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CourseController(ICourseService _courseService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var result = await _courseService.GetAllCoursesAsync();
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseDetails(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _courseService.GetCourseDetailsAsync(id, userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromForm] CreateCourseDto dto)
        {
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value });
            Console.WriteLine($"User Claims: {System.Text.Json.JsonSerializer.Serialize(userClaims)}");
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _courseService.CreateCourseAsync(dto, instructorId);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetCourseDetails), new { id = result.Data.Id }, result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromForm] UpdateCourseDto dto)
        {
            dto.Id = id;
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _courseService.UpdateCourseAsync(dto, instructorId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _courseService.DeleteCourseAsync(id, instructorId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet("my-courses")]
        public async Task<IActionResult> GetInstructorCourses()
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _courseService.GetInstructorCoursesAsync(instructorId);

            return Ok(result);
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet("{id}/details-for-instructor")]
        public async Task<IActionResult> GetCourseDetailsForInstructor(int id)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _courseService.GetCourseWithLessonsAndStudentsAsync(id, instructorId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost("{id}/students")]
        public async Task<IActionResult> AddStudentToCourse(int id, [FromBody] string studentEmail)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _courseService.AddStudentToCourseAsync(id, studentEmail, instructorId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpDelete("{id}/students/{studentEmail}")]
        public async Task<IActionResult> RemoveStudentFromCourse(int id, string studentEmail)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _courseService.RemoveStudentFromCourseAsync(id, studentEmail, instructorId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("my-enrollments")]
        public async Task<IActionResult> GetStudentCourses()
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _courseService.GetStudentEnrolledCoursesAsync(studentId);

            return Ok(result);
        }
    }

}
