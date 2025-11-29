using Application.DTOs.Dashboard;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IDashboardService
    {
        Task<BaseResponse<InstructorDashboardDto>> GetInstructorDashboardAsync(string instructorId);
        Task<BaseResponse<StudentDashboardDto>> GetStudentDashboardAsync(string studentId);
        Task<BaseResponse<CourseStatisticsDto>> GetCourseStatisticsAsync(int courseId, string instructorId);
    }
}
