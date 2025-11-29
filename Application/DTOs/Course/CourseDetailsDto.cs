using Application.DTOs.Lesson;
namespace Application.DTOs.Course;

public class CourseDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string CoverImageUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public DateTime? DiscountEndDate { get; set; }
    public decimal FinalPrice { get; set; }
    public bool IsPublished { get; set; }
    public string InstructorName { get; set; }
    public int TotalLessons { get; set; }
    public int TotalStudents { get; set; }
    public bool IsEnrolled { get; set; }
    public List<LessonListDto> Lessons { get; set; }
}
