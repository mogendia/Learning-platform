using Application.DTOs.Live;
using Domain.Common;

namespace Application.Services
{
    public interface ILiveSessionService
    {
        Task<BaseResponse<LiveSessionCreateResponseDto>> CreateSessionAsync(CreateLiveSessionDto dto, string instructorId);
        Task<BaseResponse<LiveSessionDetailsDto>> StartSessionAsync(Guid sessionId, string instructorId);
        Task<BaseResponse<LiveSessionDetailsDto>> EndSessionAsync(Guid sessionId, string instructorId);
        Task<BaseResponse<LiveSessionDetailsDto>> GetSessionAsync(Guid sessionId, string userId, bool isInstructor);
        Task<BaseResponse<LiveSessionDetailsDto>> GetActiveSessionByCourseAsync(int courseId, string userId, bool isInstructor);
        Task<BaseResponse<LiveJoinResponseDto>> JoinSessionAsync(Guid sessionId, string userId, bool isInstructor);
    }
}
