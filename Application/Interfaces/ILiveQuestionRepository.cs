using Domain.Entities;

namespace Application.Interfaces
{
    public interface ILiveQuestionRepository : IGenericRepository<LiveQuestion>
    {
        Task<LiveQuestion> GetByQuestionIdAsync(Guid questionId);
        Task<IEnumerable<LiveQuestion>> GetQuestionsForSessionAsync(Guid sessionId);
        Task<IEnumerable<LiveQuestion>> GetPendingQuestionsAsync(Guid sessionId);
    }
}
