using Application.DTOs.Dashboard;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    public static class EnrollmentMappingExtensions
    {
        public static EnrolledCourseDto ToEnrolledCourseDto(
            this Enrollment enrollment,
            int completedLessons,
            int totalLessons)
        {
            var progressPercentage = totalLessons > 0
                ? (double)completedLessons / totalLessons * 100
                : 0;

            return new EnrolledCourseDto
            {
                CourseId = enrollment.CourseId,
                CourseTitle = enrollment.Course?.Title,
                CoverImageUrl = enrollment.Course?.CoverImageUrl,
                InstructorName = enrollment.Course?.Instructor?.FullName,
                CompletedLessons = completedLessons,
                TotalLessons = totalLessons,
                ProgressPercentage = progressPercentage,
                EnrolledAt = enrollment.EnrolledAt
            };
        }

        public static StudentProgressDto ToStudentProgressDto(
            this Enrollment enrollment,
            int completedLessons,
            int totalLessons)
        {
            var progressPercentage = totalLessons > 0
                ? (double)completedLessons / totalLessons * 100
                : 0;

            return new StudentProgressDto
            {
                StudentId = enrollment.StudentId,
                StudentName = enrollment.Student?.FullName,
                Email = enrollment.Student?.Email,
                PhoneNumber = enrollment.Student?.PhoneNumber,
                EnrolledAt = enrollment.EnrolledAt,
                CompletedLessons = completedLessons,
                TotalLessons = totalLessons,
                ProgressPercentage = progressPercentage
            };
        }
    }
}
