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
    public class LessonProgressRepository : GenericRepository<LessonProgress>, ILessonProgressRepository
    {
        public LessonProgressRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<LessonProgress> GetProgressAsync(string studentId, int lessonId)
        {
            return await _context.LessonProgresses
                .FirstOrDefaultAsync(p => p.StudentId == studentId && p.LessonId == lessonId);
        }

        public async Task<IEnumerable<LessonProgress>> GetCourseProgressAsync(string studentId, int courseId)
        {
            return await _context.LessonProgresses
                .Include(p => p.Lesson)
                .Where(p => p.StudentId == studentId && p.Lesson.CourseId == courseId)
                .ToListAsync();
        }
    }
}
