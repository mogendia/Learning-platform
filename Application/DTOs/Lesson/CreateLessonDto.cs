using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Lesson
{
    public class CreateLessonDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile VideoFile { get; set; }
        public IFormFile Thumbnail { get; set; }
        public int CourseId { get; set; }
        public int OrderIndex { get; set; }
    }
}
