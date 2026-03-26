using Application.DTOs.Live;
using Application.Interfaces;
using Application.Services;
using Domain.Common;
using Domain.Entities;
using System;

namespace Application.ImpServices
{
    public class LiveSessionService : ILiveSessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILiveTokenService _tokenService;
        private readonly ILiveSessionNotifier _notifier;
        public LiveSessionService(IUnitOfWork unitOfWork, ILiveTokenService tokenService, ILiveSessionNotifier notifier)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _notifier = notifier;
        }

        public async Task<BaseResponse<LiveSessionCreateResponseDto>> CreateSessionAsync(CreateLiveSessionDto dto, string instructorId)
        {
            try
            {
                var course = await _unitOfWork.Courses.GetByIdAsync(dto.CourseId);
                if (course == null)
                    return BaseResponse<LiveSessionCreateResponseDto>.FailureResult("Course Not Found");
                if (course.InstructorId != instructorId)
                    return BaseResponse<LiveSessionCreateResponseDto>.FailureResult("Unauthorized");

                // Auto-end any stale Live sessions for this course
                var staleSessions = await _unitOfWork.LiveSessions.FindAsync(
                    s => s.CourseId == dto.CourseId && s.Status == LiveSessionStatus.Live);
                foreach (var stale in staleSessions)
                {
                    stale.Status = LiveSessionStatus.Ended;
                    stale.EndedAt = DateTime.UtcNow;
                    stale.UpdatedAt = DateTime.UtcNow;
                    await _unitOfWork.LiveSessions.UpdateAsync(stale);
                }

                var session = new LiveSession
                {
                    SessionId = Guid.NewGuid(),
                    CourseId = dto.CourseId,
                    InstructorId = instructorId,
                    Status = LiveSessionStatus.Scheduled,
                    StreamRoomId = $"course-{dto.CourseId}-live-{Guid.NewGuid():N}",
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.LiveSessions.AddAsync(session);
                await _unitOfWork.SaveAsync();

                var response = new LiveSessionCreateResponseDto
                {
                    SessionId = session.SessionId,
                    Status = session.Status.ToString(),
                    StreamRoomId = session.StreamRoomId
                };

                return BaseResponse<LiveSessionCreateResponseDto>.SuccessResult(response, "Live Session Created");
            }
            catch (Exception ex)
            {
                return BaseResponse<LiveSessionCreateResponseDto>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<LiveSessionDetailsDto>> StartSessionAsync(Guid sessionId, string instructorId)
        {
            try
            {
                var session = await _unitOfWork.LiveSessions.GetBySessionIdAsync(sessionId);
                if (session == null)
                    return BaseResponse<LiveSessionDetailsDto>.FailureResult("Session Not Found");
                if (session.InstructorId != instructorId)
                    return BaseResponse<LiveSessionDetailsDto>.FailureResult("Unauthorized");

                session.Status = LiveSessionStatus.Live;
                session.StartedAt = DateTime.UtcNow;
                session.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.LiveSessions.UpdateAsync(session);
                await _unitOfWork.SaveAsync();

                await _notifier.NotifySessionUpdatedAsync(session.SessionId, session.Status.ToString(), session.StartedAt, session.EndedAt);

                return BaseResponse<LiveSessionDetailsDto>.SuccessResult(MapDetails(session), "Live Session Started");
            }
            catch (Exception ex)
            {
                return BaseResponse<LiveSessionDetailsDto>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<LiveSessionDetailsDto>> EndSessionAsync(Guid sessionId, string instructorId)
        {
            try
            {
                var session = await _unitOfWork.LiveSessions.GetBySessionIdAsync(sessionId);
                if (session == null)
                    return BaseResponse<LiveSessionDetailsDto>.FailureResult("Session Not Found");
                if (session.InstructorId != instructorId)
                    return BaseResponse<LiveSessionDetailsDto>.FailureResult("Unauthorized");

                session.Status = LiveSessionStatus.Ended;
                session.EndedAt = DateTime.UtcNow;
                session.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.LiveSessions.UpdateAsync(session);
                await _unitOfWork.SaveAsync();

                await _notifier.NotifySessionUpdatedAsync(session.SessionId, session.Status.ToString(), session.StartedAt, session.EndedAt);

                return BaseResponse<LiveSessionDetailsDto>.SuccessResult(MapDetails(session), "Live Session Ended");
            }
            catch (Exception ex)
            {
                return BaseResponse<LiveSessionDetailsDto>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<LiveSessionDetailsDto>> GetSessionAsync(Guid sessionId, string userId, bool isInstructor)
        {
            try
            {
                var session = await _unitOfWork.LiveSessions.GetBySessionIdAsync(sessionId);
                if (session == null)
                    return BaseResponse<LiveSessionDetailsDto>.FailureResult("Session Not Found");

                if (!isInstructor)
                {
                    var enrolled = await _unitOfWork.Enrollments.IsEnrolledAsync(userId, session.CourseId);
                    if (!enrolled)
                        return BaseResponse<LiveSessionDetailsDto>.FailureResult("Unauthorized");
                }

                return BaseResponse<LiveSessionDetailsDto>.SuccessResult(MapDetails(session));
            }
            catch (Exception ex)
            {
                return BaseResponse<LiveSessionDetailsDto>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<LiveSessionDetailsDto>> GetActiveSessionByCourseAsync(int courseId, string userId, bool isInstructor)
        {
            try
            {
                var session = await _unitOfWork.LiveSessions.GetActiveSessionForCourseAsync(courseId);
                if (session == null)
                    return BaseResponse<LiveSessionDetailsDto>.FailureResult("No active session");

                if (!isInstructor)
                {
                    var enrolled = await _unitOfWork.Enrollments.IsEnrolledAsync(userId, courseId);
                    if (!enrolled)
                        return BaseResponse<LiveSessionDetailsDto>.FailureResult("Unauthorized");
                }

                return BaseResponse<LiveSessionDetailsDto>.SuccessResult(MapDetails(session));
            }
            catch (Exception ex)
            {
                return BaseResponse<LiveSessionDetailsDto>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<LiveJoinResponseDto>> JoinSessionAsync(Guid sessionId, string userId, bool isInstructor)
        {
            try
            {
                var session = await _unitOfWork.LiveSessions.GetBySessionIdAsync(sessionId);
                if (session == null)
                    return BaseResponse<LiveJoinResponseDto>.FailureResult("Session Not Found");

                if (!isInstructor)
                {
                    var enrolled = await _unitOfWork.Enrollments.IsEnrolledAsync(userId, session.CourseId);
                    if (!enrolled)
                        return BaseResponse<LiveJoinResponseDto>.FailureResult("Unauthorized");
                }

                var participant = await _unitOfWork.LiveParticipants.GetParticipantAsync(sessionId, userId);
                if (participant == null)
                {
                    participant = new LiveParticipant
                    {
                        SessionId = sessionId,
                        UserId = userId,
                        Role = isInstructor ? LiveParticipantRole.Instructor : LiveParticipantRole.Student,
                        CanSpeak = isInstructor,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.LiveParticipants.AddAsync(participant);
                    await _unitOfWork.SaveAsync();
                }

                var role = participant.Role.ToString();
                var token = await _tokenService.GenerateTokenAsync(session.StreamRoomId, userId, role, participant.CanSpeak);

                var response = new LiveJoinResponseDto
                {
                    RoomId = session.StreamRoomId,
                    Token = token,
                    Role = role,
                    CanSpeak = participant.CanSpeak
                };

                return BaseResponse<LiveJoinResponseDto>.SuccessResult(response);
            }
            catch (Exception ex)
            {
                return BaseResponse<LiveJoinResponseDto>.FailureResult($"Error: {ex.Message}");
            }
        }

        private static LiveSessionDetailsDto MapDetails(LiveSession session)
        {
            return new LiveSessionDetailsDto
            {
                SessionId = session.SessionId,
                CourseId = session.CourseId,
                InstructorId = session.InstructorId,
                StreamRoomId = session.StreamRoomId,
                Status = session.Status.ToString(),
                StartedAt = session.StartedAt,
                EndedAt = session.EndedAt
            };
        }
    }
}
