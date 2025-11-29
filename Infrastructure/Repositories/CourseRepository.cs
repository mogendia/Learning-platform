using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Course> GetCourseWithLessonsAsync(int courseId)
        {

            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Lessons)
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }

        public async Task<IEnumerable<Course>> GetInstructorCourseAsync(string instructorId)
        {
            return await _context.Courses
                 .Where(i => i.InstructorId == instructorId)
                 .Include(l => l.Lessons)
                 .Include(e => e.Enrollments)
                 .ToListAsync();
        }


        public async Task<IEnumerable<Course>> GetPublishedCoursesAsync()
        {

            return await _context.Courses
                .Where(c => c.IsPublished)
                .Include(i => i.Instructor)
                .Include(l => l.Lessons)
                .Include(e => e.Enrollments)
                .Include(e => e.Enrollments)
                .ToListAsync();
        }

        public async Task<Course> GetCourseWithStudentsAndLessonsAsync(int courseId, string instructorId)
        {
            return await _context.Courses
                .Include(c => c.Lessons)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.Id == courseId && c.InstructorId == instructorId);
        }

    }
}
