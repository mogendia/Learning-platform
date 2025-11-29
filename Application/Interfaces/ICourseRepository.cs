using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<Course> GetCourseWithLessonsAsync(int courseId);
        Task<IEnumerable<Course>> GetInstructorCourseAsync(string instructorId);
        Task<IEnumerable<Course>> GetPublishedCoursesAsync();
        Task<Course> GetCourseWithStudentsAndLessonsAsync(int courseId, string instructorId);

    }
}
