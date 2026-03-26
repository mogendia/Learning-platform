using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LiveSessionRepository : GenericRepository<LiveSession>, ILiveSessionRepository
    {
        public LiveSessionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<LiveSession> GetBySessionIdAsync(Guid sessionId)
        {
            return await _context.LiveSessions
                .Include(s => s.Course)
                .Include(s => s.Instructor)
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);
        }

        public async Task<LiveSession> GetByIdWithDetailsAsync(int id)
        {
            return await _context.LiveSessions
                .Include(s => s.Course)
                .Include(s => s.Instructor)
                .Include(s => s.Questions)
                .Include(s => s.Participants)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<LiveSession> GetActiveSessionForCourseAsync(int courseId)
        {
            return await _context.LiveSessions
                .Where(s => s.CourseId == courseId && s.Status == LiveSessionStatus.Live)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
