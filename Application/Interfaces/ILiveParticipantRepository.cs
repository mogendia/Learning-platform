using Domain.Entities;

namespace Application.Interfaces
{
    public interface ILiveParticipantRepository : IGenericRepository<LiveParticipant>
    {
        Task<LiveParticipant> GetParticipantAsync(Guid sessionId, string userId);
        Task<IEnumerable<LiveParticipant>> GetParticipantsForSessionAsync(Guid sessionId);
    }
}
