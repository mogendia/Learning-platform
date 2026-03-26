using Domain.Entities;

namespace Application.Interfaces
{
    public interface ILiveSessionRepository : IGenericRepository<LiveSession>
    {
        Task<LiveSession> GetBySessionIdAsync(Guid sessionId);
        Task<LiveSession> GetByIdWithDetailsAsync(int id);
        Task<LiveSession> GetActiveSessionForCourseAsync(int courseId);
    }
}
