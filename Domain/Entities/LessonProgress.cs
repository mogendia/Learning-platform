using System;

namespace Domain.Entities
{
    public class LessonProgress : BaseEntity
    {
        public int WatchedMinutes { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime LastWatchedAt { get; set; } = DateTime.UtcNow;

        public string StudentId { get; set; }
        public int LessonId { get; set; }

        // العلاقات مع الكيانات الأخرى
        public virtual ApplicationUser Student { get; set; }
        public virtual Lesson Lesson { get; set; }
    }
}
