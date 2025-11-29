using Application.DTOs.Lesson;
using Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Course
{
    public class CourseInstructorDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<LessonDto> Lessons { get; set; }
        public List<StudentDto> Students { get; set; }
    }
}
