using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LiveParticipantRepository : GenericRepository<LiveParticipant>, ILiveParticipantRepository
    {
        public LiveParticipantRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<LiveParticipant> GetParticipantAsync(Guid sessionId, string userId)
        {
            return await _context.LiveParticipants
                .FirstOrDefaultAsync(p => p.SessionId == sessionId && p.UserId == userId);
        }

        public async Task<IEnumerable<LiveParticipant>> GetParticipantsForSessionAsync(Guid sessionId)
        {
            return await _context.LiveParticipants
                .Where(p => p.SessionId == sessionId)
                .ToListAsync();
        }
    }
}
