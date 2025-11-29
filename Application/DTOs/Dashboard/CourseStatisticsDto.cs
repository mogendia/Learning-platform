using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Dashboard
{
    public class CourseStatisticsDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int TotalEnrolled { get; set; }
        public decimal Revenue { get; set; }
        public List<StudentProgressDto> Students { get; set; }
    }
}
