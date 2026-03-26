using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LiveQuestionRepository : GenericRepository<LiveQuestion>, ILiveQuestionRepository
    {
        public LiveQuestionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<LiveQuestion> GetByQuestionIdAsync(Guid questionId)
        {
            return await _context.LiveQuestions
                .Include(q => q.Student)
                .FirstOrDefaultAsync(q => q.QuestionId == questionId);
        }

        public async Task<IEnumerable<LiveQuestion>> GetQuestionsForSessionAsync(Guid sessionId)
        {
            return await _context.LiveQuestions
                .Include(q => q.Student)
                .Where(q => q.SessionId == sessionId)
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<LiveQuestion>> GetPendingQuestionsAsync(Guid sessionId)
        {
            return await _context.LiveQuestions
                .Include(q => q.Student)
                .Where(q => q.SessionId == sessionId && q.Status == LiveQuestionStatus.Pending)
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();
        }
    }
}
