using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Dashboard
{
    public class EnrolledCourseDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CoverImageUrl { get; set; }
        public string InstructorName { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
        public double ProgressPercentage { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}
