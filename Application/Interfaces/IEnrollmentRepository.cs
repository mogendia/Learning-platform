using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
    {
        Task<Enrollment> GetEnrollmentAsync(string studentId, int courseId);
        Task<IEnumerable<Enrollment>> GetStudentEnrollmentsAsync(string studentId);
        Task<IEnumerable<Enrollment>> GetCourseEnrollmentsAsync(int courseId);
        Task<bool> IsEnrolledAsync(string studentId, int courseId);
    }
}
