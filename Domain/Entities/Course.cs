namespace Domain.Entities;

public class Course : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string CoverImageUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPercentage{ get; set; }
    public DateTime? DiscountEndDate { get; set; }
    public bool IsPublished { get; set; }
    public string InstructorId { get; set; }

    public ApplicationUser Instructor { get; set; }
    public virtual ICollection<Enrollment> Enrollments { get; set; }
    public virtual ICollection<Lesson> Lessons{ get; set; }


    public decimal FinalPrice => DiscountPercentage.HasValue &&
                                 DiscountEndDate.HasValue &&
                                 DiscountEndDate > DateTime.UtcNow
        ? Price - (Price * DiscountPercentage.Value / 100)
        : Price;


}
