using Application.DTOs.Course;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ICourseService
    {
        Task<BaseResponse<CourseDetailsDto>> CreateCourseAsync(CreateCourseDto dto, string instructorId);
        Task<BaseResponse<CourseDetailsDto>> UpdateCourseAsync(UpdateCourseDto dto, string instructorId);
        Task<BaseResponse<bool>> DeleteCourseAsync(int courseId, string instructorId);
        Task<BaseResponse<CourseDetailsDto>> GetCourseDetailsAsync(int courseId, string? userId);
        Task<BaseResponse<List<CourseListDto>>> GetAllCoursesAsync();
        Task<BaseResponse<List<CourseListDto>>> GetInstructorCoursesAsync(string instructorId);
        Task<BaseResponse<CourseInstructorDetailsDto>> GetCourseWithLessonsAndStudentsAsync(int courseId, string instructorId);
        Task<BaseResponse<bool>> AddStudentToCourseAsync(int courseId, string studentEmail, string instructorId);
        Task<BaseResponse<bool>> RemoveStudentFromCourseAsync(int courseId, string studentEmail, string instructorId);
        Task<BaseResponse<List<CourseListDto>>> GetStudentEnrolledCoursesAsync(string studentId);
    }
}
