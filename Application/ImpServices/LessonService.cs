using Application.DTOs.Lesson;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ImpServices
{
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public LessonService(
            IUnitOfWork unitOfWork,
            ICloudinaryService cloudinaryService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BaseResponse<LessonDetailsDto>> CreateLessonAsync(CreateLessonDto dto, string instructorId)
        {
            try
            {
                var course = await _unitOfWork.Courses.GetByIdAsync(dto.CourseId);

                if (course == null)
                    return BaseResponse<LessonDetailsDto>.FailureResult("Course not found");

                if (course.InstructorId != instructorId)
                    return BaseResponse<LessonDetailsDto>.FailureResult("Unauthorized");

                // رفع الفيديو والصورة على Cloudinary
                var videoUrl = await _cloudinaryService.UploadVideoAsync(dto.VideoFile);
                var thumbnailUrl = await _cloudinaryService.UploadImageAsync(dto.Thumbnail);

                // استخدام AutoMapper
                var lesson = _mapper.Map<Lesson>(dto);
                lesson.VideoUrl = videoUrl;
                lesson.ThumbnailUrl = thumbnailUrl;

                await _unitOfWork.Lessons.AddAsync(lesson);
                await _unitOfWork.SaveAsync();

                var lessonDto = _mapper.Map<LessonDetailsDto>(lesson);

                return BaseResponse<LessonDetailsDto>.SuccessResult(lessonDto, "Lesson created successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<LessonDetailsDto>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DeleteLessonAsync(int lessonId, string instructorId)
        {
            try
            {
                var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId);

                if (lesson == null)
                    return BaseResponse<bool>.FailureResult("Lesson not found");

                var course = await _unitOfWork.Courses.GetByIdAsync(lesson.CourseId);

                if (course.InstructorId != instructorId)
                    return BaseResponse<bool>.FailureResult("Unauthorized");

                await _unitOfWork.Lessons.DeleteAsync(lesson);
                await _unitOfWork.SaveAsync();

                return BaseResponse<bool>.SuccessResult(true, "Lesson deleted successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<LessonDetailsDto>> GetLessonDetailsAsync(int lessonId, string userId)
        {
            try
            {
                var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId);

                if (lesson == null)
                    return BaseResponse<LessonDetailsDto>.FailureResult("Lesson not found");

                // ✅ تشيك لو المستخدم Instructor
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return BaseResponse<LessonDetailsDto>.FailureResult("User not found");

                var roles = await _userManager.GetRolesAsync(user);
                var isInstructor = roles.Contains("Instructor");

                // لو Instructor، يديله access مباشرة
                if (!isInstructor)
                {
                    // لو مش Instructor، تشيك على الـ Enrollment
                    var isEnrolled = await _unitOfWork.Enrollments.IsEnrolledAsync(userId, lesson.CourseId);

                    if (!isEnrolled)
                        return BaseResponse<LessonDetailsDto>.FailureResult("You must enroll in the course first");
                }

                var lessonDto = _mapper.Map<LessonDetailsDto>(lesson);

                return BaseResponse<LessonDetailsDto>.SuccessResult(lessonDto);
            }
            catch (Exception ex)
            {
                return BaseResponse<LessonDetailsDto>.FailureResult($"Error: {ex.Message}");
            }
        }
        public async Task<BaseResponse<bool>> UpdateProgressAsync(int lessonId, string studentId, int watchedMinutes)
        {
            try
            {
                var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId);

                if (lesson == null)
                    return BaseResponse<bool>.FailureResult("Lesson not found");

                var progress = await _unitOfWork.LessonProgresses.GetProgressAsync(studentId, lessonId);

                if (progress == null)
                {
                    progress = new LessonProgress
                    {
                        StudentId = studentId,
                        LessonId = lessonId,
                        WatchedMinutes = watchedMinutes,
                        IsCompleted = watchedMinutes >= lesson.DurationInMins * 0.9,
                        LastWatchedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.LessonProgresses.AddAsync(progress);
                }
                else
                {
                    progress.WatchedMinutes = Math.Max(progress.WatchedMinutes, watchedMinutes);
                    progress.IsCompleted = progress.WatchedMinutes >= lesson.DurationInMins * 0.9;
                    progress.LastWatchedAt = DateTime.UtcNow;

                    await _unitOfWork.LessonProgresses.UpdateAsync(progress);
                }

                await _unitOfWork.SaveAsync();

                return BaseResponse<bool>.SuccessResult(true, "Progress updated successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<IEnumerable<LessonListDto>>> GetAllLessonsAsync()
        {
            try
            {
                var lessons = await _unitOfWork.Lessons.GetAll();
                var lessonDtos = lessons.OrderBy(l => ((Domain.Entities.Lesson)l).OrderIndex).Select(l => _mapper.Map<LessonListDto>(l));
                return BaseResponse<IEnumerable<LessonListDto>>.SuccessResult(lessonDtos);
            }
            catch (Exception ex)
            {
                return BaseResponse<IEnumerable<LessonListDto>>.FailureResult($"Error: {ex.Message}");
            }
        }
        public async Task<BaseResponse<IEnumerable<LessonListDto>>> GetLessonsByCourseIdAsync(int courseId)
        {
            try
            {
                var lessons = await _unitOfWork.Lessons.GetCourseWithLessonAsync(courseId);
                var lessonDtos = lessons.Select(l => _mapper.Map<LessonListDto>(l));
                return BaseResponse<IEnumerable<LessonListDto>>.SuccessResult(lessonDtos);
            }
            catch (Exception ex)
            {
                return BaseResponse<IEnumerable<LessonListDto>>.FailureResult($"Error: {ex.Message}");
            }
        }
        public async Task<BaseResponse<IEnumerable<LessonListDto>>> GetLessonsByCourseIdAsync(int courseId, string instructorId)
        {
            try
            {
                // Verify instructor owns the course
                var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
                if (course == null)
                    return BaseResponse<IEnumerable<LessonListDto>>.FailureResult("Course not found");

                if (course.InstructorId != instructorId)
                    return BaseResponse<IEnumerable<LessonListDto>>.FailureResult("Unauthorized");

                // Get lessons
                var lessons = await _unitOfWork.Lessons.GetCourseWithLessonAsync(courseId);
                var lessonDtos = lessons.Select(l => _mapper.Map<LessonListDto>(l));

                return BaseResponse<IEnumerable<LessonListDto>>.SuccessResult(lessonDtos);
            }
            catch (Exception ex)
            {
                return BaseResponse<IEnumerable<LessonListDto>>.FailureResult($"Error: {ex.Message}");
            }
        }
    }
}
