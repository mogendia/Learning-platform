using Microsoft.AspNetCore.Http;
namespace Application.DTOs.Course;

public class CreateCourseDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile CoverImage { get; set; }
    public decimal Price { get; set; }
}
