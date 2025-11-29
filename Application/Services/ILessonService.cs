using Application.DTOs.Lesson;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ILessonService
    {
        Task<BaseResponse<LessonDetailsDto>> CreateLessonAsync(CreateLessonDto dto, string instructorId);
        Task<BaseResponse<bool>> DeleteLessonAsync(int lessonId, string instructorId);
        Task<BaseResponse<LessonDetailsDto>> GetLessonDetailsAsync(int lessonId, string userId);
        Task<BaseResponse<bool>> UpdateProgressAsync(int lessonId, string studentId, int watchedSeconds);
        Task<BaseResponse<IEnumerable<LessonListDto>>> GetAllLessonsAsync();
        Task<BaseResponse<IEnumerable<LessonListDto>>> GetLessonsByCourseIdAsync(int courseId);
        Task<BaseResponse<IEnumerable<LessonListDto>>> GetLessonsByCourseIdAsync(int courseId, string instructorId);
    }
}
