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
    public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Lesson>> GetCourseWithLessonAsync(int courseId)
        {
            return await _context.Lessons
                .Where(c=>c.CourseId == courseId)
                .OrderBy(o=>o.OrderIndex)
                .ToListAsync();
        }
    }
}
