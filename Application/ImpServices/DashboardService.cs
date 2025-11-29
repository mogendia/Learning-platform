using Application.DTOs.Dashboard;
using Application.Interfaces;
using Application.Mapping;
using Application.Services;
using AutoMapper;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ImpServices
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DashboardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<InstructorDashboardDto>> GetInstructorDashboardAsync(string instructorId)
        {
            try
            {
                var courses = await _unitOfWork.Courses.GetInstructorCourseAsync(instructorId);

                var totalStudents = courses.SelectMany(c => c.Enrollments).Select(e => e.StudentId).Distinct().Count();
                var totalRevenue = courses.SelectMany(c => c.Enrollments).Sum(e => e.PaidAmount);

                var courseStatistics = new List<CourseStatisticsDto>();

                foreach (var course in courses)
                {
                    var enrollments = await _unitOfWork.Enrollments.GetCourseEnrollmentsAsync(course.Id);
                    var students = new List<StudentProgressDto>();

                    foreach (var enrollment in enrollments)
                    {
                        var progresses = await _unitOfWork.LessonProgresses.GetCourseProgressAsync(enrollment.StudentId, course.Id);
                        var completedLessons = progresses.Count(p => p.IsCompleted);
                        var totalLessons = course.Lessons.Count;

                        // استخدام Extension Method
                        students.Add(enrollment.ToStudentProgressDto(completedLessons, totalLessons));
                    }

                    var stat = _mapper.Map<CourseStatisticsDto>(course);
                    stat.Students = students;
                    courseStatistics.Add(stat);
                }

                var dashboard = new InstructorDashboardDto
                {
                    TotalCourses = courses.Count(),
                    TotalStudents = totalStudents,
                    TotalRevenue = totalRevenue,
                    CourseStatistics = courseStatistics
                };

                return BaseResponse<InstructorDashboardDto>.SuccessResult(dashboard);
            }
            catch (Exception ex)
            {
                return BaseResponse<InstructorDashboardDto>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<StudentDashboardDto>> GetStudentDashboardAsync(string studentId)
        {
            try
            {
                var enrollments = await _unitOfWork.Enrollments.GetStudentEnrollmentsAsync(studentId);

                var enrolledCourses = new List<EnrolledCourseDto>();

                foreach (var enrollment in enrollments)
                {
                    var progresses = await _unitOfWork.LessonProgresses.GetCourseProgressAsync(studentId, enrollment.CourseId);
                    var completedLessons = progresses.Count(p => p.IsCompleted);
                    var totalLessons = enrollment.Course.Lessons.Count;

                    // استخدام Extension Method
                    enrolledCourses.Add(enrollment.ToEnrolledCourseDto(completedLessons, totalLessons));
                }

                var dashboard = new StudentDashboardDto
                {
                    TotalEnrolledCourses = enrollments.Count(),
                    CompletedCourses = enrolledCourses.Count(c => c.ProgressPercentage >= 90),
                    EnrolledCourses = enrolledCourses
                };

                return BaseResponse<StudentDashboardDto>.SuccessResult(dashboard);
            }
            catch (Exception ex)
            {
                return BaseResponse<StudentDashboardDto>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<CourseStatisticsDto>> GetCourseStatisticsAsync(int courseId, string instructorId)
        {
            try
            {
                var course = await _unitOfWork.Courses.GetCourseWithLessonsAsync(courseId);

                if (course == null)
                    return BaseResponse<CourseStatisticsDto>.FailureResult("Course not found");

                if (course.InstructorId != instructorId)
                    return BaseResponse<CourseStatisticsDto>.FailureResult("Unauthorized");

                var enrollments = await _unitOfWork.Enrollments.GetCourseEnrollmentsAsync(courseId);
                var students = new List<StudentProgressDto>();

                foreach (var enrollment in enrollments)
                {
                    var progresses = await _unitOfWork.LessonProgresses.GetCourseProgressAsync(enrollment.StudentId, courseId);
                    var completedLessons = progresses.Count(p => p.IsCompleted);
                    var totalLessons = course.Lessons.Count;

                    students.Add(enrollment.ToStudentProgressDto(completedLessons, totalLessons));
                }

                var statistics = _mapper.Map<CourseStatisticsDto>(course);
                statistics.Students = students;

                return BaseResponse<CourseStatisticsDto>.SuccessResult(statistics);
            }
            catch (Exception ex)
            {
                return BaseResponse<CourseStatisticsDto>.FailureResult($"Error: {ex.Message}");
            }
        }
    }
}
