    using Application.DTOs.Live;
    using Application.Interfaces;
    using Application.Services;
    using Domain.Common;
    using Domain.Entities;
    using System;
    using System.Linq;

    namespace Application.ImpServices
    {
        public class LiveQuestionService : ILiveQuestionService
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILiveSessionNotifier _notifier;
            public LiveQuestionService(IUnitOfWork unitOfWork, ILiveSessionNotifier notifier)
            {
                _unitOfWork = unitOfWork;
                _notifier = notifier;
            }

            public async Task<BaseResponse<LiveQuestionDto>> RaiseQuestionAsync(Guid sessionId, CreateLiveQuestionDto dto, string studentId)
            {
                try
                {
                    var session = await _unitOfWork.LiveSessions.GetBySessionIdAsync(sessionId);
                    if (session == null)
                        return BaseResponse<LiveQuestionDto>.FailureResult("Session Not Found");

                    var enrolled = await _unitOfWork.Enrollments.IsEnrolledAsync(studentId, session.CourseId);
                    if (!enrolled)
                        return BaseResponse<LiveQuestionDto>.FailureResult("Unauthorized");

                    var question = new LiveQuestion
                    {
                        QuestionId = Guid.NewGuid(),
                        SessionId = sessionId,
                        StudentId = studentId,
                        Message = dto.Message,
                        Status = LiveQuestionStatus.Pending,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.LiveQuestions.AddAsync(question);
                    await _unitOfWork.SaveAsync();

                    var response = MapQuestion(question, string.Empty);
                    await _notifier.NotifyQuestionCreatedAsync(sessionId, response);
                    return BaseResponse<LiveQuestionDto>.SuccessResult(response, "Question Raised");
                }
                catch (Exception ex)
                {
                    return BaseResponse<LiveQuestionDto>.FailureResult($"Error: {ex.Message}");
                }
            }

            public async Task<BaseResponse<List<LiveQuestionDto>>> GetQuestionsAsync(Guid sessionId, string instructorId)
            {
                try
                {
                    var session = await _unitOfWork.LiveSessions.GetBySessionIdAsync(sessionId);
                    if (session == null)
                        return BaseResponse<List<LiveQuestionDto>>.FailureResult("Session Not Found");
                    if (session.InstructorId != instructorId)
                        return BaseResponse<List<LiveQuestionDto>>.FailureResult("Unauthorized");

                    var questions = await _unitOfWork.LiveQuestions.GetQuestionsForSessionAsync(sessionId);
                    var response = questions
                        .Select(q => MapQuestion(q, q.Student?.FullName))
                        .ToList();

                    return BaseResponse<List<LiveQuestionDto>>.SuccessResult(response);
                }
                catch (Exception ex)
                {
                    return BaseResponse<List<LiveQuestionDto>>.FailureResult($"Error: {ex.Message}");
                }
            }

            public async Task<BaseResponse<LiveQuestionDto>> ApproveQuestionAsync(Guid sessionId, Guid questionId, string instructorId)
            {
                try
                {
                    var session = await _unitOfWork.LiveSessions.GetBySessionIdAsync(sessionId);
                    if (session == null)
                        return BaseResponse<LiveQuestionDto>.FailureResult("Session Not Found");
                    if (session.InstructorId != instructorId)
                        return BaseResponse<LiveQuestionDto>.FailureResult("Unauthorized");

                    var question = await _unitOfWork.LiveQuestions.GetByQuestionIdAsync(questionId);
                    if (question == null || question.SessionId != sessionId)
                        return BaseResponse<LiveQuestionDto>.FailureResult("Question Not Found");

                    question.Status = LiveQuestionStatus.Approved;
                    question.UpdatedAt = DateTime.UtcNow;

                    var participant = await _unitOfWork.LiveParticipants.GetParticipantAsync(sessionId, question.StudentId);
                    if (participant == null)
                    {
                        participant = new LiveParticipant
                        {
                            SessionId = sessionId,
                            UserId = question.StudentId,
                            Role = LiveParticipantRole.Student,
                            CanSpeak = true,
                            CreatedAt = DateTime.UtcNow
                        };
                        await _unitOfWork.LiveParticipants.AddAsync(participant);
                    }
                    else
                    {
                        participant.CanSpeak = true;
                        participant.UpdatedAt = DateTime.UtcNow;
                        await _unitOfWork.LiveParticipants.UpdateAsync(participant);
                    }

                    await _unitOfWork.LiveQuestions.UpdateAsync(question);
                    await _unitOfWork.SaveAsync();

                    var response = MapQuestion(question, question.Student?.FullName ?? string.Empty);
                    await _notifier.NotifyQuestionApprovedAsync(sessionId, response);
                    await _notifier.NotifySpeakGrantedAsync(sessionId, question.StudentId);
                    return BaseResponse<LiveQuestionDto>.SuccessResult(response, "Question Approved");
                }
                catch (Exception ex)
                {
                    return BaseResponse<LiveQuestionDto>.FailureResult($"Error: {ex.Message}");
                }
            }

            public async Task<BaseResponse<LiveQuestionDto>> RevokeQuestionAsync(Guid sessionId, Guid questionId, string instructorId)
            {
                try
                {
                    var session = await _unitOfWork.LiveSessions.GetBySessionIdAsync(sessionId);
                    if (session == null)
                        return BaseResponse<LiveQuestionDto>.FailureResult("Session Not Found");
                    if (session.InstructorId != instructorId)
                        return BaseResponse<LiveQuestionDto>.FailureResult("Unauthorized");

                    var question = await _unitOfWork.LiveQuestions.GetByQuestionIdAsync(questionId);
                    if (question == null || question.SessionId != sessionId)
                        return BaseResponse<LiveQuestionDto>.FailureResult("Question Not Found");

                    question.Status = LiveQuestionStatus.Revoked;
                    question.UpdatedAt = DateTime.UtcNow;

                    var participant = await _unitOfWork.LiveParticipants.GetParticipantAsync(sessionId, question.StudentId);
                    if (participant != null)
                    {
                        participant.CanSpeak = false;
                        participant.UpdatedAt = DateTime.UtcNow;
                        await _unitOfWork.LiveParticipants.UpdateAsync(participant);
                    }

                    await _unitOfWork.LiveQuestions.UpdateAsync(question);
                    await _unitOfWork.SaveAsync();

                    var response = MapQuestion(question, question.Student?.FullName ?? string.Empty);
                    await _notifier.NotifyQuestionRevokedAsync(sessionId, response);
                    await _notifier.NotifySpeakRevokedAsync(sessionId, question.StudentId);
                    return BaseResponse<LiveQuestionDto>.SuccessResult(response, "Question Revoked");
                }
                catch (Exception ex)
                {
                    return BaseResponse<LiveQuestionDto>.FailureResult($"Error: {ex.Message}");
                }
            }

            private static LiveQuestionDto MapQuestion(LiveQuestion question, string studentName)
            {
                return new LiveQuestionDto
                {
                    QuestionId = question.QuestionId,
                    SessionId = question.SessionId,
                    StudentId = question.StudentId,
                    StudentName = studentName ?? string.Empty,
                    Message = question.Message,
                    Status = question.Status.ToString(),
                    CreatedAt = question.CreatedAt
                };
            }
        }
    }
