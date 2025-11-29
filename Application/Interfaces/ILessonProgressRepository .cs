using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILessonProgressRepository : IGenericRepository<LessonProgress>
    {
        Task<LessonProgress> GetProgressAsync(string studentId, int lessonId);
        Task<IEnumerable<LessonProgress>> GetCourseProgressAsync(string studentId, int courseId);
    }
}
