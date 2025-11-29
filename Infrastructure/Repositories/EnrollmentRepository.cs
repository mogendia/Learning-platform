using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Enrollment>> GetCourseEnrollmentsAsync(int courseId)
        {
            return await _context.Enrollments
                .Include(s => s.Student)
                .Where(c => c.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<Enrollment> GetEnrollmentAsync(string studentId, int courseId)
        {
            return await _context.Enrollments
                .Include(s => s.Student)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(s => s.StudentId == studentId && s.CourseId == courseId);
        }

        public async Task<IEnumerable<Enrollment>> GetStudentEnrollmentsAsync(string studentId)
        {
           return await _context.Enrollments
                .Include(c=>c.Course)
                .ThenInclude(s=>s.Lessons)
                .Include(e=>e.Course.Instructor)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<bool> IsEnrolledAsync(string studentId, int courseId)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }
    }
}
