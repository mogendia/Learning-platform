using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Lesson
{

    public class LessonListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public int DurationInMins { get; set; }
        public int OrderIndex { get; set; }
        public bool IsCompleted { get; set; }
        public int watchedMins { get; set; }
    }
}
