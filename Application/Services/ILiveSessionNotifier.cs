using Application.DTOs.Live;

namespace Application.Services
{
    public interface ILiveSessionNotifier
    {
        Task NotifySessionUpdatedAsync(Guid sessionId, string status, DateTime? startedAt, DateTime? endedAt);
        Task NotifyQuestionCreatedAsync(Guid sessionId, LiveQuestionDto question);
        Task NotifyQuestionApprovedAsync(Guid sessionId, LiveQuestionDto question);
        Task NotifyQuestionRevokedAsync(Guid sessionId, LiveQuestionDto question);
        Task NotifySpeakGrantedAsync(Guid sessionId, string studentId);
        Task NotifySpeakRevokedAsync(Guid sessionId, string studentId);
    }
}
