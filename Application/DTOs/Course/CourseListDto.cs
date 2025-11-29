namespace Application.DTOs.Course;
public class CourseListDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string CoverImageUrl { get; set; }
    public decimal Price { get; set; }
    public decimal FinalPrice { get; set; }
    public string InstructorName { get; set; }
    public int TotalLessons { get; set; }
    public int TotalStudents { get; set; }
}
