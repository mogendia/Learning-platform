using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController(IDashboardService _dashboardService) : ControllerBase
    {
        [Authorize(Roles = "Instructor")]
        [HttpGet("instructor")]
        public async Task<IActionResult> GetInstructorDashboard()
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _dashboardService.GetInstructorDashboardAsync(instructorId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("student")]
        public async Task<IActionResult> GetStudentDashboard()
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _dashboardService.GetStudentDashboardAsync(studentId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetCourseStatistics(int courseId)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _dashboardService.GetCourseStatisticsAsync(courseId, instructorId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
