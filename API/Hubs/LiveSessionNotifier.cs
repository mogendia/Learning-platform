using Application.DTOs.Live;
using Application.Services;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class LiveSessionNotifier : ILiveSessionNotifier
    {
        private readonly IHubContext<LiveSessionHub> _hubContext;

        public LiveSessionNotifier(IHubContext<LiveSessionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task NotifySessionUpdatedAsync(Guid sessionId, string status, DateTime? startedAt, DateTime? endedAt)
        {
            return _hubContext.Clients.Group(sessionId.ToString())
                .SendAsync("liveSession.updated", new
                {
                    sessionId,
                    status,
                    startedAt,
                    endedAt
                });
        }

        public Task NotifyQuestionCreatedAsync(Guid sessionId, LiveQuestionDto question)
        {
            return _hubContext.Clients.Group(sessionId.ToString())
                .SendAsync("liveQuestion.created", question);
        }

        public Task NotifyQuestionApprovedAsync(Guid sessionId, LiveQuestionDto question)
        {
            return _hubContext.Clients.Group(sessionId.ToString())
                .SendAsync("liveQuestion.approved", question);
        }

        public Task NotifyQuestionRevokedAsync(Guid sessionId, LiveQuestionDto question)
        {
            return _hubContext.Clients.Group(sessionId.ToString())
                .SendAsync("liveQuestion.revoked", question);
        }

        public Task NotifySpeakGrantedAsync(Guid sessionId, string studentId)
        {
            return _hubContext.Clients.Group(sessionId.ToString())
                .SendAsync("participant.speakGranted", new { sessionId, studentId });
        }

        public Task NotifySpeakRevokedAsync(Guid sessionId, string studentId)
        {
            return _hubContext.Clients.Group(sessionId.ToString())
                .SendAsync("participant.speakRevoked", new { sessionId, studentId });
        }
    }
}
