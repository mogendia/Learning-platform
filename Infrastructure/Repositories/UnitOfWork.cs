using Application.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ICourseRepository Courses { get; }
        public ILessonRepository Lessons { get; }
        public IEnrollmentRepository Enrollments { get; }
        public ILessonProgressRepository LessonProgresses { get; }
        public IReviewRepository Reviews { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Courses = new CourseRepository(_context);
            Lessons = new LessonRepository(_context);
            Enrollments = new EnrollmentRepository(_context);
            LessonProgresses = new LessonProgressRepository(_context);
            Reviews = new ReviewRepository(_context);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
