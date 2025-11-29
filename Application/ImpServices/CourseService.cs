using Application.DTOs.Course;
using Application.DTOs.Lesson;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Mapping;
using Application.Services;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ImpServices
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinary;
        private readonly IMapper _mapper;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public CourseService(IUnitOfWork unitOfWork, ICloudinaryService cloudinary, IMapper mapper, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _cloudinary = cloudinary;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BaseResponse<CourseDetailsDto>> CreateCourseAsync(CreateCourseDto dto, string instructorId)
        {
            try
            {
                var coverImg = await _cloudinary.UploadImageAsync(dto.CoverImage);
                var course = _mapper.Map<Course>(dto);
                course.CoverImageUrl = coverImg;
                course.InstructorId = instructorId;
                course.IsPublished = true;

                await _unitOfWork.Courses.AddAsync(course);
                await _unitOfWork.SaveAsync();

                var courseDto = _mapper.Map<CourseDetailsDto>(course);
                return BaseResponse<CourseDetailsDto>.SuccessResult(courseDto, "Course Created");
            }
            catch (Exception ex)
            {
                return BaseResponse<CourseDetailsDto>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DeleteCourseAsync(int courseId, string instructorId)
        {
            try
            {
                var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
                if (course == null)
                    return BaseResponse<bool>.FailureResult("Course Can't Found");
                if (course.InstructorId != instructorId)
                    return BaseResponse<bool>.FailureResult("Unauthorized");

                await _unitOfWork.Courses.DeleteAsync(course);
                await _unitOfWork.SaveAsync();
                return BaseResponse<bool>.SuccessResult(true, "Course deleted successfully");

            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<CourseListDto>>> GetAllCoursesAsync()
        {
            try
            {
                var course = await _unitOfWork.Courses.GetPublishedCoursesAsync();
                Console.WriteLine($"[Debug] Published courses count from repo = {course.Count()}");
                var courseDto = _mapper.Map<List<CourseListDto>>(course);
                return BaseResponse<List<CourseListDto>>.SuccessResult(courseDto);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CourseListDto>>.FailureResult($"Error: {ex.Message}");
            }

        }
        public async Task<BaseResponse<CourseDetailsDto>> GetCourseDetailsAsync(int courseId, string? userId)
        {
            try
            {
                var course = await _unitOfWork.Courses.GetCourseWithLessonsAsync(courseId);

                if (course == null)
                    return BaseResponse<CourseDetailsDto>.FailureResult("Course not found");

                var courseDto = MapToCourseDetailsDto(course);

                // تحديد حالة التسجيل وإضافة معلومات التقدم
                if (!string.IsNullOrEmpty(userId))
                {
                    // تشيك لو الطالب مسجل في الكورس
                    var isEnrolled = await _unitOfWork.Enrollments.IsEnrolledAsync(userId, courseId);
                    courseDto.IsEnrolled = isEnrolled;

                    // إذا كان مسجل، نضيف معلومات التقدم
                    if (isEnrolled)
                    {
                        var progresses = await _unitOfWork.LessonProgresses.GetCourseProgressAsync(userId, courseId);
                        var progressDict = progresses.ToDictionary(p => p.LessonId);

                        foreach (var lesson in courseDto.Lessons)
                        {
                            if (progressDict.TryGetValue(lesson.Id, out var progress))
                            {
                                lesson.IsCompleted = progress.IsCompleted;
                                lesson.watchedMins = progress.WatchedMinutes;
                            }
                        }
                    }
                }
                else
                {
                    // لو مش مسجل دخول أصلاً، IsEnrolled = false 
                    courseDto.IsEnrolled = false;
                }

                return BaseResponse<CourseDetailsDto>.SuccessResult(courseDto);
            }
            catch (Exception ex)
            {
                return BaseResponse<CourseDetailsDto>.FailureResult($"Error: {ex.Message}");
            }
        }
        public async Task<BaseResponse<List<CourseListDto>>> GetInstructorCoursesAsync(string instructorId)
        {
            try
            {
                var courses = await _unitOfWork.Courses.GetInstructorCourseAsync(instructorId);
                var courseDtos = _mapper.Map<List<CourseListDto>>(courses);

                return BaseResponse<List<CourseListDto>>.SuccessResult(courseDtos);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CourseListDto>>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<CourseDetailsDto>> UpdateCourseAsync(UpdateCourseDto dto, string instructorId)
        {
            try
            {
                var course = await _unitOfWork.Courses.GetByIdAsync(dto.Id);
                if (course == null)
                    return BaseResponse<CourseDetailsDto>.FailureResult("Course Can't Found");
                if (course.InstructorId != instructorId)
                    return BaseResponse<CourseDetailsDto>.FailureResult("Unauthorized");

                _mapper.Map(dto, course);

                if (dto.CoverImage != null)
                    course.CoverImageUrl = await _cloudinary.UploadImageAsync(dto.CoverImage);

                await _unitOfWork.Courses.UpdateAsync(course);
                await _unitOfWork.SaveAsync();

                var courseDto = _mapper.Map<CourseDetailsDto>(course);

                return BaseResponse<CourseDetailsDto>.SuccessResult(courseDto, "Course updated successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<CourseDetailsDto>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<CourseInstructorDetailsDto>> GetCourseWithLessonsAndStudentsAsync(int courseId, string instructorId)
        {
            try
            {
                var course = await _unitOfWork.Courses.GetCourseWithStudentsAndLessonsAsync(courseId, instructorId);

                if (course == null)
                    return BaseResponse<CourseInstructorDetailsDto>.FailureResult("Course not found or you are not the instructor");

                var dto = new CourseInstructorDetailsDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Lessons = course.Lessons.Select(l => new LessonDto
                    {
                        Id = l.Id,
                        Title = l.Title,
                        DurationInMinutes = l.DurationInMins
                    }).ToList(),
                    Students = course.Enrollments.Select(e => new StudentDto
                    {
                        Id = e.Student.Id,
                        Name = e.Student.FullName, // Assuming FullName exists on ApplicationUser
                        Email = e.Student.Email
                    }).ToList()
                };

                return BaseResponse<CourseInstructorDetailsDto>.SuccessResult(dto);
            }
            catch (Exception ex)
            {
                return BaseResponse<CourseInstructorDetailsDto>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> AddStudentToCourseAsync(int courseId, string studentEmail, string instructorId)
        {
            try
            {
                var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
                if (course == null)
                    return BaseResponse<bool>.FailureResult("Course not found");

                if (course.InstructorId != instructorId)
                    return BaseResponse<bool>.FailureResult("Unauthorized");

                var student = await _userManager.FindByEmailAsync(studentEmail);
                if (student == null)
                    return BaseResponse<bool>.FailureResult("Student not found");

                var isEnrolled = await _unitOfWork.Enrollments.IsEnrolledAsync(student.Id, courseId);
                if (isEnrolled)
                    return BaseResponse<bool>.FailureResult("Student is already enrolled");

                var enrollment = new Enrollment
                {
                    CourseId = courseId,
                    StudentId = student.Id,
                    PaidAmount = course.Price,
                    PaymentTransactionId = "Manual_By_Instructor_" + instructorId
                };

                await _unitOfWork.Enrollments.AddAsync(enrollment);
                await _unitOfWork.SaveAsync();

                return BaseResponse<bool>.SuccessResult(true, "Student added successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> RemoveStudentFromCourseAsync(int courseId, string studentEmail, string instructorId)
        {
            try
            {
                var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
                if (course == null)
                    return BaseResponse<bool>.FailureResult("Course not found");

                if (course.InstructorId != instructorId)
                    return BaseResponse<bool>.FailureResult("Unauthorized");

                var student = await _userManager.FindByEmailAsync(studentEmail);
                if (student == null)
                    return BaseResponse<bool>.FailureResult("Student not found");

                var enrollment = await _unitOfWork.Enrollments.GetEnrollmentAsync(student.Id, courseId);
                if (enrollment == null)
                    return BaseResponse<bool>.FailureResult("Student is not enrolled in this course");

                await _unitOfWork.Enrollments.DeleteAsync(enrollment);
                await _unitOfWork.SaveAsync();

                return BaseResponse<bool>.SuccessResult(true, "Student removed successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<CourseListDto>>> GetStudentEnrolledCoursesAsync(string studentId)
        {
            try
            {
                var enrollments = await _unitOfWork.Enrollments.GetStudentEnrollmentsAsync(studentId);
                var courses = enrollments.Select(e => e.Course).ToList();
                var courseDtos = _mapper.Map<List<CourseListDto>>(courses);

                return BaseResponse<List<CourseListDto>>.SuccessResult(courseDtos);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CourseListDto>>.FailureResult($"Error: {ex.Message}");
            }
        }

        private CourseDetailsDto MapToCourseDetailsDto(Course course)
        {
            return new CourseDetailsDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                CoverImageUrl = course.CoverImageUrl,
                Price = course.Price,
                DiscountPercentage = course.DiscountPercentage,
                DiscountEndDate = course.DiscountEndDate,
                FinalPrice = course.FinalPrice,
                IsPublished = course.IsPublished,
                InstructorName = course.Instructor?.FullName,
                TotalLessons = course.Lessons?.Count ?? 0,
                TotalStudents = course.Enrollments?.Count ?? 0,
                Lessons = course.Lessons?.Select(l => new LessonListDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    ThumbnailUrl = l.ThumbnailUrl,
                    DurationInMins = l.DurationInMins,
                    OrderIndex = l.OrderIndex
                }).ToList()
            };
        }

        private CourseListDto MapToCourseListDto(Course course)
        {
            return new CourseListDto
            {
                Id = course.Id,
                Title = course.Title,
                CoverImageUrl = course.CoverImageUrl,
                Price = course.Price,
                FinalPrice = course.FinalPrice,
                InstructorName = course.Instructor?.FullName,
                TotalLessons = course.Lessons?.Count ?? 0,
                TotalStudents = course.Enrollments?.Count ?? 0
            };
        }
    }
}
