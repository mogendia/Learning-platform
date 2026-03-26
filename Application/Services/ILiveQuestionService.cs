using Application.DTOs.Live;
using Domain.Common;

namespace Application.Services
{
    public interface ILiveQuestionService
    {
        Task<BaseResponse<LiveQuestionDto>> RaiseQuestionAsync(Guid sessionId, CreateLiveQuestionDto dto, string studentId);
        Task<BaseResponse<List<LiveQuestionDto>>> GetQuestionsAsync(Guid sessionId, string instructorId);
        Task<BaseResponse<LiveQuestionDto>> ApproveQuestionAsync(Guid sessionId, Guid questionId, string instructorId);
        Task<BaseResponse<LiveQuestionDto>> RevokeQuestionAsync(Guid sessionId, Guid questionId, string instructorId);
    }
}
