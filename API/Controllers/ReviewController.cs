using Application.DTOs.Review;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(IReviewService _review) : ControllerBase
    {
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveReview()
        {
            var result = await _review.GetActiveReviewAsync();
            return Ok(result);
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var result = await _review.GetReviewsAsync();
            return Ok(result);
        }
        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public async Task<IActionResult> CreateTestimonial([FromForm] CreateReviewDto dto)
        {
            var result = await _review.CreateReviewsAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        [Authorize(Roles = "Instructor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromForm] UpdateReviewDto dto)
        {
            dto.Id = id;
            var result = await _review.UpdateReviewsAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestimonial(int id)
        {
            var result = await _review.DeleteReviewAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

