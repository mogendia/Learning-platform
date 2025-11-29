using System.Collections.Generic;

namespace Domain.Entities
{
    public class Lesson : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public int DurationInMins { get; set; }
        public int OrderIndex { get; set; }
        public int CourseId { get; set; }

        // العلاقات مع الكيانات الأخرى
        public virtual Course Course { get; set; }
        public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new HashSet<LessonProgress>();
    }
}