using Microsoft.AspNetCore.Http;
namespace Application.DTOs.Course;

public class UpdateCourseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile? CoverImage { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public DateTime? DiscountEndDate { get; set; }
    public bool IsPublished { get; set; }
}
